using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;

namespace OrderManagement.UnitTest.ProductHandlers
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateProductCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidProduct_UpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                Id = productId,
                Name = "Updated Product",
                Price = 149.99m,
                StockCount = 20,
                ProductCategoryId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Description = "Updated description"
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Original Product",
                Price = 99.99m,
                StockCount = 10,
                ProductCategoryId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Description = "Original description",
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            // Setup the repository mock to return the existing product
            _repositoryMock.Setup(r => r.GetByIdAsync(productId.ToString()))
                           .ReturnsAsync(existingProduct);

            // Setup the mapper mock to map the command to the updated product entity
            _mapperMock.Setup(m => m.Map<Product>(command))
                       .Returns(new Product
                       {
                           Id = productId,
                           Name = command.Name,
                           Price = command.Price,
                           StockCount = command.StockCount,
                           ProductCategoryId = command.ProductCategoryId,
                           CompanyId = command.CompanyId,
                           Description = command.Description,
                           CreatedDate = existingProduct.CreatedDate,
                           LastUpdateDate = DateTime.UtcNow // Updated last update date
                       });

            // Setup the unit of work mock
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify that GetByIdAsync was called
            _repositoryMock.Verify(r => r.GetByIdAsync(productId.ToString()), Times.Once);

            // Verify that UpdateAsync was called with the updated product
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Product>(p =>
                p.Id == productId &&
                p.Name == command.Name &&
                p.Price == command.Price &&
                p.StockCount == command.StockCount &&
                p.ProductCategoryId == command.ProductCategoryId &&
                p.CompanyId == command.CompanyId &&
                p.Description == command.Description &&
                p.CreatedDate == existingProduct.CreatedDate &&
                p.LastUpdateDate != existingProduct.LastUpdateDate // Ensure LastUpdateDate is updated
            )), Times.Once);

            // Verify that SaveChangesAsync was called
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidProductId_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand { Id = productId };

            _repositoryMock.Setup(r => r.GetByIdAsync(productId.ToString()))
                           .ReturnsAsync((Product)null); // Simulating not found scenario

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(productId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
