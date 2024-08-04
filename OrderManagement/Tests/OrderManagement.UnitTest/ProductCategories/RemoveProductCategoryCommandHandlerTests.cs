using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.ProductCategory;
using OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;
using OrderManagement.Application.CQRS.Handlers.OrderHandlers;
using OrderManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.ProductCategoryHandlers
{
    public class RemoveProductCategoryCommandHandlerTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly RemoveProductCategoryCommandHandler _handler;

        public RemoveProductCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cachingServiceMock = new Mock<ICachingService>();

            _handler = new RemoveProductCategoryCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingProductCategory_ReturnsSuccessResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var productCategory = new ProductCategory { Id = productCategoryId, Name = "Test Category" };

            _repositoryMock.Setup(r => r.GetByIdAsync(productCategoryId.ToString()))
                           .ReturnsAsync(productCategory);

            ApiResponse<NoContent> expectedResponse = ApiResponse<NoContent>.Success(ResponseType.Success);

            // Act
            var result = await _handler.Handle(new RemoveProductCategoryCommand (productCategoryId.ToString()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetByIdAsync(productCategoryId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(productCategory), Times.Once);

            // Verify unit of work method call
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            // Verify caching service method calls
            _cachingServiceMock.Verify(c => c.RemoveAsync($"ProductCategory_{productCategoryId}"), Times.Once);
            _cachingServiceMock.Verify(c => c.RemoveAsync("ProductCategoryList"), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingProductCategory_ReturnsFailResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(productCategoryId.ToString()))
                           .ReturnsAsync((ProductCategory)null);

            ApiResponse<NoContent> expectedResponse = ApiResponse<NoContent>.Fail("Product Category not found", ResponseType.Fail);

            // Act
            var result = await _handler.Handle(new RemoveProductCategoryCommand (productCategoryId.ToString()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetByIdAsync(productCategoryId.ToString()), Times.Once);

            // Verify unit of work method call (should not be called)
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);

            // Verify caching service method calls (should not be called)
            _cachingServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
