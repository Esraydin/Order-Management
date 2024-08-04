using AutoMapper;
using Moq;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using OrderManagement.Application.CQRS.Queries.ProductCategoryQueries;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using ProductCategoryManagement.Application.CQRS.Handlers.ProductCategoryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.ProductCategoryHandlers
{
    public class GetProductCategoryQueryHandlerTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly GetProductCategoryQueryHandler _handler;

        public GetProductCategoryQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _cachingServiceMock = new Mock<ICachingService>();

            _handler = new GetProductCategoryQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WithCachedData_ReturnsSuccessResponse()
        {
            // Arrange
            var cachedProductCategories = new List<ProductCategory>
            {
                new ProductCategory { Id = Guid.NewGuid(), Name = "Category 1", CreatedDate = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Category 2", CreatedDate = DateTime.UtcNow }
            };

            _cachingServiceMock.Setup(c => c.GetAsync<List<ProductCategory>>("ProductCategoryList"))
                               .ReturnsAsync(cachedProductCategories);

            var cachedResult = cachedProductCategories.Select(pc => new GetProductCategoryQueryResult
            {
                Id = pc.Id,
                Name = pc.Name,
                CreatedDate = pc.CreatedDate,
                Description = pc.Description,
                LastUpdateDate = pc.LastUpdateDate
            }).ToList();

            _mapperMock.Setup(m => m.Map<List<GetProductCategoryQueryResult>>(cachedProductCategories))
                       .Returns(cachedResult);

            var getProductCategoryQuery = new GetProductCategoryQuery();

            // Act
            var result = await _handler.Handle(getProductCategoryQuery, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);
            Assert.Equal(cachedResult.Count, result.Data.Count);

            for (int i = 0; i < cachedResult.Count; i++)
            {
                Assert.Equal(cachedResult[i].Id, result.Data[i].Id);
                Assert.Equal(cachedResult[i].Name, result.Data[i].Name);
                Assert.Equal(cachedResult[i].CreatedDate, result.Data[i].CreatedDate);
                Assert.Equal(cachedResult[i].Description, result.Data[i].Description);
                Assert.Equal(cachedResult[i].LastUpdateDate, result.Data[i].LastUpdateDate);
            }

            // Verify caching service method call
            _cachingServiceMock.Verify(c => c.GetAsync<List<ProductCategory>>("ProductCategoryList"), Times.Once);

            // Verify mapper method call
            _mapperMock.Verify(m => m.Map<List<GetProductCategoryQueryResult>>(cachedProductCategories), Times.Once);

            // Verify repository method call (should not be called since data is retrieved from cache)
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WithNoCachedData_ReturnsSuccessResponse()
        {
            // Arrange
            var productCategories = new List<ProductCategory>
            {
                new ProductCategory { Id = Guid.NewGuid(), Name = "Category 1", CreatedDate = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Category 2", CreatedDate = DateTime.UtcNow }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(productCategories);

            var mappedResult = productCategories.Select(pc => new GetProductCategoryQueryResult
            {
                Id = pc.Id,
                Name = pc.Name,
                CreatedDate = pc.CreatedDate,
                Description = pc.Description,
                LastUpdateDate = pc.LastUpdateDate
            }).ToList();

            _mapperMock.Setup(m => m.Map<List<GetProductCategoryQueryResult>>(productCategories))
                       .Returns(mappedResult);

            _cachingServiceMock.Setup(c => c.SetAsync("ProductCategoryList", productCategories, It.IsAny<TimeSpan>()));

            var getProductCategoryQuery = new GetProductCategoryQuery();

            // Act
            var result = await _handler.Handle(getProductCategoryQuery, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);
            Assert.Equal(mappedResult.Count, result.Data.Count);

            for (int i = 0; i < mappedResult.Count; i++)
            {
                Assert.Equal(mappedResult[i].Id, result.Data[i].Id);
                Assert.Equal(mappedResult[i].Name, result.Data[i].Name);
                Assert.Equal(mappedResult[i].CreatedDate, result.Data[i].CreatedDate);
                Assert.Equal(mappedResult[i].Description, result.Data[i].Description);
                Assert.Equal(mappedResult[i].LastUpdateDate, result.Data[i].LastUpdateDate);
            }

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

            // Verify caching service method calls
            _cachingServiceMock.Verify(c => c.GetAsync<List<ProductCategory>>("ProductCategoryList"), Times.Once);
            _cachingServiceMock.Verify(c => c.SetAsync("ProductCategoryList", productCategories, It.IsAny<TimeSpan>()), Times.Once);

            // Verify mapper method call
            _mapperMock.Verify(m => m.Map<List<GetProductCategoryQueryResult>>(productCategories), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNoDataInRepository_ReturnsFailResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(new List<ProductCategory>());

            var getProductCategoryQuery = new GetProductCategoryQuery();

            // Act
            var result = await _handler.Handle(getProductCategoryQuery, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

            // Verify caching service method calls (should not be called since no data to cache)
            _cachingServiceMock.Verify(c => c.GetAsync<List<ProductCategory>>("ProductCategoryList"), Times.Once); // Cacheden bir kez veri alınmalı
            _cachingServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<List<ProductCategory>>(), It.IsAny<TimeSpan>()), Times.Never);

            // Verify mapper method call (should not be called since no data to map)
            _mapperMock.Verify(m => m.Map<List<GetProductCategoryQueryResult>>(It.IsAny<List<ProductCategory>>()), Times.Never);
        }

    }
}
