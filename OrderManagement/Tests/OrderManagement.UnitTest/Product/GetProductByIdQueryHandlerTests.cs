using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.ResponseModel;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;
using ProductManagement.Application.CQRS.Queries.ProductQueries;

namespace OrderManagement.UnitTest.ProductHandlers
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetProductByIdQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var query = new GetProductByIdQuery (productId.ToString());

            var productEntity = new Product
            {
                Id = productId,
                Name = "Test Product",
                CompanyId = Guid.NewGuid(),
                Description = "Test Description",
                Price = 99.99m,
                StockCount = 100,
                ProductCategoryId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            var expectedResult = ApiResponse<GetProductByIdQueryResult>.Success(new GetProductByIdQueryResult
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                CompanyId = productEntity.CompanyId,
                Description = productEntity.Description,
                Price = productEntity.Price,
                StockCount = productEntity.StockCount,
                ProductCategoryId = productEntity.ProductCategoryId,
                CreatedDate = productEntity.CreatedDate,
                LastUpdateDate = productEntity.LastUpdateDate
            });

            _repositoryMock.Setup(r => r.GetByIdAsync(productId.ToString()))
                           .ReturnsAsync(productEntity);

            _mapperMock.Setup(m => m.Map<GetProductByIdQueryResult>(productEntity))
                       .Returns(new GetProductByIdQueryResult
                       {
                           Id = productEntity.Id,
                           Name = productEntity.Name,
                           CompanyId = productEntity.CompanyId,
                           Description = productEntity.Description,
                           Price = productEntity.Price,
                           StockCount = productEntity.StockCount,
                           ProductCategoryId = productEntity.ProductCategoryId,
                           CreatedDate = productEntity.CreatedDate,
                           LastUpdateDate = productEntity.LastUpdateDate
                       });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResult.Data.Id, result.Data.Id);
            Assert.Equal(expectedResult.Data.Name, result.Data.Name);
            Assert.Equal(expectedResult.Data.CompanyId, result.Data.CompanyId);
            Assert.Equal(expectedResult.Data.Description, result.Data.Description);
            Assert.Equal(expectedResult.Data.Price, result.Data.Price);
            Assert.Equal(expectedResult.Data.StockCount, result.Data.StockCount);
            Assert.Equal(expectedResult.Data.ProductCategoryId, result.Data.ProductCategoryId);
            Assert.Equal(expectedResult.Data.CreatedDate, result.Data.CreatedDate);
            Assert.Equal(expectedResult.Data.LastUpdateDate, result.Data.LastUpdateDate);

            _repositoryMock.Verify(r => r.GetByIdAsync(productId.ToString()), Times.Once);
            _mapperMock.Verify(m => m.Map<GetProductByIdQueryResult>(productEntity), Times.Once);
        }
    }
}
