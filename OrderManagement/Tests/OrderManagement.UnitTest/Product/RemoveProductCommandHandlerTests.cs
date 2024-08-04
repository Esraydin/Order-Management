using Moq;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;

namespace OrderManagement.UnitTest.ProductHandlers
{
    public class RemoveProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RemoveProductCommandHandler _handler;

        public RemoveProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RemoveProductCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidProductId_RemovesProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new RemoveProductCommand(productId.ToString());

            var productToRemove = new Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 99.99m,
                StockCount = 10,
                ProductCategoryId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                CompanyId = Guid.NewGuid(),
                LastUpdateDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(productId.ToString()))
                           .ReturnsAsync(productToRemove);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(productId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(productToRemove), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidProductId_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new RemoveProductCommand(productId.ToString());

            _repositoryMock.Setup(r => r.GetByIdAsync(productId.ToString()))
                           .ReturnsAsync((Product)null); // Simulating not found scenario

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(productId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

    }
}
