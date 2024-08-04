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

namespace OrderManagement.UnitTest.Companies
{
    public class CreateCompanyCommandHandlerTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cacheServiceMock;
        private readonly CreateCompanyCommandHandler _handler;

        public CreateCompanyCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<ICachingService>();
            _handler = new CreateCompanyCommandHandler(_repositoryMock.Object, _mapperMock.Object, _unitOfWorkMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessResponse_WhenCompanyCreatedSuccessfully()
        {
            // Arrange
            var command = new CreateCompanyCommand
            {
                Name = "Test Company",
                Description = "Test Description",
                UserId = Guid.NewGuid()
            };

            var companyEntity = new Company
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                UserId = command.UserId,
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            };

            _mapperMock.Setup(m => m.Map<Company>(command))
                .Returns(companyEntity);

            //_unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            //    .ReturnsAsync(true); // Simulating successful save

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify repository interactions
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            // Verify caching interactions
            _cacheServiceMock.Verify(c => c.RemoveAsync("CompanyList"), Times.Once);
            _cacheServiceMock.Verify(c => c.SetAsync($"Company_{companyEntity.Id}", companyEntity, It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
