using AutoMapper;
using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Handlers.CompanyHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTest.CompanyHandlers
{
    public class UpdateCompanyCommandHandlerTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly UpdateCompanyCommandHandler _handler;

        public UpdateCompanyCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cachingServiceMock = new Mock<ICachingService>();
            _handler = new UpdateCompanyCommandHandler(_repositoryMock.Object, _mapperMock.Object, _unitOfWorkMock.Object, _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UpdatesCompanyInDatabaseAndCache()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var updateCommand = new UpdateCompanyCommand
            {
                Id = companyId,//ToString(),
                Name = "Updated Company Name",
                Description = "Updated Description",
                UserId = Guid.NewGuid()//.ToString()
            };
            var company = new Company
            {
                Id = companyId,
                Name = "Original Company Name",
                Description = "Original Description",
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                           .ReturnsAsync(company);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            _mapperMock.Verify(m => m.Map(updateCommand, company), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(company), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            var cacheKey = $"Company_{companyId}";
            _cachingServiceMock.Verify(c => c.RemoveAsync(cacheKey), Times.Once);
            _cachingServiceMock.Verify(c => c.SetAsync(cacheKey, company, TimeSpan.FromMinutes(30)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFailWhenCompanyNotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var updateCommand = new UpdateCompanyCommand
            {
                Id = companyId,
                Name = "Updated Company Name",
                Description = "Updated Description",
                UserId = Guid.NewGuid() // UserId için doğrudan Guid nesnesi oluşturulmalı
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(companyId.ToString()))
                           .ReturnsAsync((Company)null);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            //Assert.Equal("Company not found", result.ErrorDto.Message);

            _mapperMock.Verify(m => m.Map(It.IsAny<UpdateCompanyCommand>(), It.IsAny<Company>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Company>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
            _cachingServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never);
            _cachingServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        }

    }
}