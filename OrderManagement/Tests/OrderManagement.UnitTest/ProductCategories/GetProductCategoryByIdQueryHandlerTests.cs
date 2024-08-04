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
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.ProductCategoryHandlers
{
    public class GetProductCategoryByIdQueryHandlerTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly GetProductCategoryByIdQueryHandler _handler;

        public GetProductCategoryByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _cachingServiceMock = new Mock<ICachingService>();

            _handler = new GetProductCategoryByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cachingServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var getProductCategoryQuery = new GetProductCategoryByIdQuery(productCategoryId.ToString());

            var productCategoryEntity = new ProductCategory
            {
                Id = productCategoryId,
                Name = "Test Category",
                Description = "Test Description",
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(productCategoryId.ToString()))
               .ReturnsAsync(productCategoryEntity);

            _mapperMock.Setup(m => m.Map<GetProductCategoryByIdQueryResult>(productCategoryEntity))
           .Returns(new GetProductCategoryByIdQueryResult
           {
               Id = productCategoryEntity.Id,
               Name = productCategoryEntity.Name,
               Description = productCategoryEntity.Description,
               CreatedDate = productCategoryEntity.CreatedDate,
               LastUpdateDate = productCategoryEntity.LastUpdateDate
           });

            // Act
            var result = await _handler.Handle(getProductCategoryQuery, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);

            var responseData = result.Data;
            Assert.Equal(productCategoryEntity.Id, responseData.Id);
            Assert.Equal(productCategoryEntity.Name, responseData.Name);
            Assert.Equal(productCategoryEntity.Description, responseData.Description);
            Assert.Equal(productCategoryEntity.CreatedDate, responseData.CreatedDate);
            Assert.Equal(productCategoryEntity.LastUpdateDate, responseData.LastUpdateDate);

            // Verify repository method calls
            _repositoryMock.Verify(r => r.GetByIdAsync(productCategoryId.ToString()), Times.Once);

            // Verify caching service method calls
            _cachingServiceMock.Verify(c => c.GetAsync<ProductCategory>($"ProductCategory_{productCategoryId}"), Times.Once);
            _cachingServiceMock.Verify(c => c.SetAsync($"ProductCategory_{productCategoryId}", productCategoryEntity, It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsNotFoundResponse()
        {
            // Arrange
            var invalidProductId = Guid.NewGuid();
            var getProductCategoryQuery = new GetProductCategoryByIdQuery(invalidProductId.ToString());

            _repositoryMock.Setup(r => r.GetByIdAsync(invalidProductId.ToString()))
                           .ReturnsAsync((ProductCategory)null); // Simulate not found scenario

            // Act
            var result = await _handler.Handle(getProductCategoryQuery, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            //Assert.Equal("Product Category not found", result.Message);

            // Verify repository method call
            _repositoryMock.Verify(r => r.GetByIdAsync(invalidProductId.ToString()), Times.Once);

            // Verify caching service method calls
            _cachingServiceMock.Verify(c => c.GetAsync<ProductCategory>($"ProductCategory_{invalidProductId}"), Times.Once); // Should still check cache
            _cachingServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProductCategory>(), It.IsAny<TimeSpan>()), Times.Never); // Should not set cache for not found item
        }
    }
}
