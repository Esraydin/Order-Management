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
using Xunit;

namespace OrderManagement.UnitTest.Companies
{
    public class GetCompanyByIdQueryHandlerTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICachingService> _cacheServiceMock;
        private readonly GetCompanyByIdQueryHandler _handler;

        public GetCompanyByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICachingService>();
            _handler = new GetCompanyByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCompanyFromCache_WhenCompanyExistsInCache()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Test Company",
                Description = "Test Description",
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                UserId = Guid.NewGuid()
            };
            var cachedCompanyResult = new GetCompanyByIdQueryResult
            {
                Id = companyId,
                Name = "Test Company",
                Description = "Test Description",
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                UserId = Guid.NewGuid()
            };

            _cacheServiceMock.Setup(c => c.GetAsync<Company>($"Company_{companyId}"))
                .ReturnsAsync(company);

            _mapperMock.Setup(m => m.Map<GetCompanyByIdQueryResult>(company))
                .Returns(cachedCompanyResult);

            var query = new GetCompanyByIdQuery(companyId.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyId, result.Data.Id);
            Assert.Equal("Test Company", result.Data.Name);
        }

        [Fact]
        public async Task Handle_ReturnsCompanyFromDatabase_WhenCompanyNotExistsInCache()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Test Company",
                Description = "Test Description",
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                UserId = Guid.NewGuid()
            };
            var companyResult = new GetCompanyByIdQueryResult
            {
                Id = companyId,
                Name = "Test Company",
                Description = "Test Description",
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                UserId = Guid.NewGuid()
            };

            _cacheServiceMock.Setup(c => c.GetAsync<Company>($"Company_{companyId}"))
                .ReturnsAsync((Company)null);

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                .ReturnsAsync(company);

            _mapperMock.Setup(m => m.Map<GetCompanyByIdQueryResult>(company))
                .Returns(companyResult);

            var query = new GetCompanyByIdQuery(companyId.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyId, result.Data.Id);
            Assert.Equal("Test Company", result.Data.Name);

            _cacheServiceMock.Verify(c => c.SetAsync($"Company_{companyId}", company, TimeSpan.FromMinutes(30)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFail_WhenCompanyNotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            _cacheServiceMock.Setup(c => c.GetAsync<Company>($"Company_{companyId}"))
                .ReturnsAsync((Company)null);

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                .ReturnsAsync((Company)null);

            var query = new GetCompanyByIdQuery(companyId.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            //Assert.Equal("Company not found", result.ErrorDto.Message);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
        }
    }
}
