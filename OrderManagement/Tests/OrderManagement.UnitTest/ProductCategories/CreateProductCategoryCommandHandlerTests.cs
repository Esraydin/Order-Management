using AutoMapper;
using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;
using OrderManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using ProductCategoryManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.ProductCategoryHandlers
{
    public class CreateProductCategoryCommandHandlerTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly CreateProductCategoryCommandHandler _handler;

        public CreateProductCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cachingServiceMock = new Mock<ICachingService>();

            _handler = new CreateProductCategoryCommandHandler(_repositoryMock.Object, _mapperMock.Object, _unitOfWorkMock.Object, _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidProductCategory_ReturnsSuccessResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var createCommand = new CreateProductCategoryCommand
            {
                Name = "Test Category",
                Description = "Test Description"
            };

            var productCategoryEntity = new ProductCategory
            {
                Id = productCategoryId,
                Name = createCommand.Name,
                Description = createCommand.Description
            };

            _mapperMock.Setup(m => m.Map<ProductCategory>(createCommand))
                       .Returns(productCategoryEntity);

            // Act
            var result = await _handler.Handle(createCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify repository methods
            _repositoryMock.Verify(r => r.AddAsync(productCategoryEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            // Verify caching service methods
            _cachingServiceMock.Verify(c => c.RemoveAsync("ProductCategoryList"), Times.Once);
            _cachingServiceMock.Verify(c => c.SetAsync($"ProductCategory_{productCategoryId}", productCategoryEntity, It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
