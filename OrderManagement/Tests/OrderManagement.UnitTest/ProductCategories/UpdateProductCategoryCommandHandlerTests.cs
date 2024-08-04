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
    public class UpdateProductCategoryCommandHandlerTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductCategoryCommandHandler _handler;

        public UpdateProductCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cachingServiceMock = new Mock<ICachingService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateProductCategoryCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object,
                _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingProductCategory_ReturnsSuccessResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var request = new UpdateProductCategoryCommand
            {
                Id = productCategoryId,
                Name = "Updated Category",
                Description = "Updated description"
            };

            var existingProductCategory = new ProductCategory
            {
                Id = productCategoryId,
                Name = "Original Category",
                Description = "Original description"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(productCategoryId.ToString()))
                           .ReturnsAsync(existingProductCategory);

            ApiResponse<NoContent> expectedResponse = ApiResponse<NoContent>.Success(ResponseType.Success);

            _mapperMock.Setup(m => m.Map(request, existingProductCategory))
                       .Returns(existingProductCategory); // Assuming mapping returns the updated entity

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetByIdAsync(productCategoryId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(existingProductCategory), Times.Once);

            // Verify unit of work method call
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            // Verify caching service method calls
            _cachingServiceMock.Verify(c => c.RemoveAsync($"ProductCategory_{productCategoryId}"), Times.Once);
            _cachingServiceMock.Verify(c => c.SetAsync($"ProductCategory_{productCategoryId}", existingProductCategory, TimeSpan.FromMinutes(30)), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingProductCategory_ReturnsFailResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var request = new UpdateProductCategoryCommand
            {
                Id = productCategoryId,
                Name = "Updated Category",
                Description = "Updated description"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(productCategoryId.ToString()))
                           .ReturnsAsync((ProductCategory)null);

            ApiResponse<NoContent> expectedResponse = ApiResponse<NoContent>.Fail("Product Category not found", ResponseType.Fail);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            //Assert.Equal("Product Category not found", result.Message);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetByIdAsync(productCategoryId.ToString()), Times.Once);

            // Verify unit of work method call (should not be called)
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);

            // Verify caching service method calls (should not be called)
            _cachingServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never);
            _cachingServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProductCategory>(), It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}
