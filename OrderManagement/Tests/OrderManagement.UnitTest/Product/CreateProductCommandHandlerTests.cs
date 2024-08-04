using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;

namespace OrderManagement.UnitTest.ProductHandlers
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CreateProductCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidProduct_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new CreateProductCommand
            {
                Name = "Test Product",
                CompanyId = Guid.NewGuid(),
                Description = "Test Description",
                Price = 99.99m,
                StockCount = 100,
                ProductCategoryId = Guid.NewGuid()
            };

            ApiResponse<NoContent> expectedResponse = ApiResponse<NoContent>.Success(ResponseType.Success);

            // Assuming mapping returns the product entity
            var productEntity = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CompanyId = request.CompanyId,
                Description = request.Description,
                Price = request.Price,
                StockCount = request.StockCount,
                ProductCategoryId = request.ProductCategoryId
            };

            _mapperMock.Setup(m => m.Map<Product>(request))
                       .Returns(productEntity);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify repository method call
            _repositoryMock.Verify(r => r.AddAsync(productEntity), Times.Once);

            // Verify unit of work method call
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
