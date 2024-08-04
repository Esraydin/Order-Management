using AutoMapper;
using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Handlers.CompanyHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.CompanyHandlers
{
    public class RemoveCompanyCommandHandlerTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cacheServiceMock;
        private readonly RemoveCompanyCommandHandler _handler;

        public RemoveCompanyCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<ICachingService>();
            _handler = new RemoveCompanyCommandHandler(_repositoryMock.Object, _mapperMock.Object, _unitOfWorkMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_RemovesCompanyFromDatabaseAndCache()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var removeCommand = new RemoveCompanyCommand(companyId.ToString());
            var company = new Company
            {
                Id = companyId,
                Name = "Test Company",
                Description = "Test Description",
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                           .ReturnsAsync(company);

            // Act
            var result = await _handler.Handle(removeCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            _repositoryMock.Verify(r => r.RemoveAsync(company), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            var cacheKey = $"Company_{companyId}";
            _cacheServiceMock.Verify(c => c.RemoveAsync(cacheKey), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync("CompanyList"), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFailWhenCompanyNotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var removeCommand = new RemoveCompanyCommand(companyId.ToString());

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                           .ReturnsAsync((Company)null);

            // Act
            var result = await _handler.Handle(removeCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            //Assert.Equal("Company not found", result.ErrorDto.Message);

            _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Company>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
            _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
