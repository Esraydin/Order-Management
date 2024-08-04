using AutoMapper;
using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Handlers.CompanyHandlers;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.Companies
{
    public class GetCompanyQueryHandlerTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICachingService> _cacheServiceMock;
        private readonly GetCompanyQueryHandler _handler;

        public GetCompanyQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICachingService>();
            _handler = new GetCompanyQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCompaniesFromCache_WhenCachedDataExists()
        {
            // Arrange
            var cachedCompanies = new List<Company>
            {
                new Company { Id = Guid.NewGuid(), Name = "Company 1", Description = "Description 1", CreatedDate = DateTime.Now, LastUpdateDate = DateTime.Now, UserId = Guid.NewGuid() },
                new Company { Id = Guid.NewGuid(), Name = "Company 2", Description = "Description 2", CreatedDate = DateTime.Now, LastUpdateDate = DateTime.Now, UserId = Guid.NewGuid() }
            };

            var cachedResults = cachedCompanies.Select(c => new GetCompanyQueryResult
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
                LastUpdateDate = c.LastUpdateDate,
                UserId = c.UserId
            }).ToList();

            _cacheServiceMock.Setup(c => c.GetAsync<List<Company>>("CompanyList"))
                .ReturnsAsync(cachedCompanies);

            _mapperMock.Setup(m => m.Map<List<GetCompanyQueryResult>>(cachedCompanies))
                .Returns(cachedResults);

            var query = new GetCompanyQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(cachedCompanies.Count, result.Data.Count);
            Assert.Equal("Company 1", result.Data[0].Name);
            Assert.Equal("Company 2", result.Data[1].Name);
        }

        [Fact]
        public async Task Handle_ReturnsCompaniesFromDatabase_WhenCachedDataNotExists()
        {
            // Arrange
            var companiesFromDb = new List<Company>
            {
                new Company { Id = Guid.NewGuid(), Name = "Company 1", Description = "Description 1", CreatedDate = DateTime.Now, LastUpdateDate = DateTime.Now, UserId = Guid.NewGuid() },
                new Company { Id = Guid.NewGuid(), Name = "Company 2", Description = "Description 2", CreatedDate = DateTime.Now, LastUpdateDate = DateTime.Now, UserId = Guid.NewGuid() }
            };

            var companyResults = companiesFromDb.Select(c => new GetCompanyQueryResult
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
                LastUpdateDate = c.LastUpdateDate,
                UserId = c.UserId
            }).ToList();

            _cacheServiceMock.Setup(c => c.GetAsync<List<Company>>("CompanyList"))
                .ReturnsAsync((List<Company>)null);

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(companiesFromDb);

            _mapperMock.Setup(m => m.Map<List<GetCompanyQueryResult>>(companiesFromDb))
                .Returns(companyResults);

            var query = new GetCompanyQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companiesFromDb.Count, result.Data.Count);
            Assert.Equal("Company 1", result.Data[0].Name);
            Assert.Equal("Company 2", result.Data[1].Name);

            _cacheServiceMock.Verify(c => c.SetAsync("CompanyList", companiesFromDb, TimeSpan.FromMinutes(30)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFail_WhenNoCompaniesFound()
        {
            // Arrange
            _cacheServiceMock.Setup(c => c.GetAsync<List<Company>>("CompanyList"))
                .ReturnsAsync((List<Company>)null);

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Company>());

            var query = new GetCompanyQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            //Assert.Equal("No companies found", result.ErrorDto.Message);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
        }
    }
}
