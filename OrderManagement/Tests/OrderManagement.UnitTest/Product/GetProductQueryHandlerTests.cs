using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.ResponseModel;
using ProductManagement.Application.CQRS.Handlers.ProductHandlers;
using ProductManagement.Application.CQRS.Queries.ProductQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.ProductHandlers
{
    public class GetProductQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductQueryHandler _handler;

        public GetProductQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetProductQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 99.99m,
                    ProductCategoryId = Guid.NewGuid(),
                    StockCount = 10,
                    CreatedDate = DateTime.UtcNow,
                    CompanyId = Guid.NewGuid(),
                    LastUpdateDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 2",
                    Description = "Description 2",
                    Price = 49.99m,
                    ProductCategoryId = Guid.NewGuid(),
                    StockCount = 5,
                    CreatedDate = DateTime.UtcNow,
                    CompanyId = Guid.NewGuid(),
                    LastUpdateDate = DateTime.UtcNow
                }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(products);

            _mapperMock.Setup(m => m.Map<List<GetProductQueryResult>>(products))
                       .Returns(products.Select(p => new GetProductQueryResult
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Description = p.Description,
                           Price = p.Price,
                           ProductCategoryId = p.ProductCategoryId,
                           StockCount = p.StockCount,
                           CreatedDate = p.CreatedDate,
                           CompanyId = p.CompanyId,
                           LastUpdateDate = p.LastUpdateDate
                       }).ToList());

            var query = new GetProductQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data);
            Assert.Equal(products.Count, result.Data.Count);

            foreach (var expectedProduct in products)
            {
                var actualProduct = result.Data.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.NotNull(actualProduct);
                Assert.Equal(expectedProduct.Name, actualProduct.Name);
                Assert.Equal(expectedProduct.Description, actualProduct.Description);
                Assert.Equal(expectedProduct.Price, actualProduct.Price);
                Assert.Equal(expectedProduct.ProductCategoryId, actualProduct.ProductCategoryId);
                Assert.Equal(expectedProduct.StockCount, actualProduct.StockCount);
                Assert.Equal(expectedProduct.CreatedDate, actualProduct.CreatedDate);
                Assert.Equal(expectedProduct.CompanyId, actualProduct.CompanyId);
                Assert.Equal(expectedProduct.LastUpdateDate, actualProduct.LastUpdateDate);
            }

            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetProductQueryResult>>(products), Times.Once);
        }
    }
}
