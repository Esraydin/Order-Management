using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Handlers.OrderHandlers;
using OrderManagement.Application.CQRS.Queries.OrderQueries;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.OrderHandlers
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderByIdQueryHandler _handler;

        public GetOrderByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOrderByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsOrderById()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var query = new GetOrderByIdQuery(orderId.ToString());
            var orderEntity = new Order
            {
                Id = orderId,
                Name = "Test Order",
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                OrderCount = 5,
                OrderStatus = OrderStatusEnum.Pending,
                UnitPrice = 10.0m,
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };
            var expectedApiResponse = ApiResponse<GetOrderByIdQueryResult>.Success(new GetOrderByIdQueryResult
            {
                Id = orderEntity.Id,
                Name = orderEntity.Name,
                UserId = orderEntity.UserId,
                CompanyId = orderEntity.CompanyId,
                OrderCount = orderEntity.OrderCount,
                OrderStatus = orderEntity.OrderStatus,
                UnitPrice = orderEntity.UnitPrice,
                CreatedDate = orderEntity.CreatedDate,
                LastUpdateDate = orderEntity.LastUpdateDate
            });

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync(orderEntity);

            _mapperMock.Setup(m => m.Map<GetOrderByIdQueryResult>(orderEntity))
                       .Returns(new GetOrderByIdQueryResult
                       {
                           Id = orderEntity.Id,
                           Name = orderEntity.Name,
                           UserId = orderEntity.UserId,
                           CompanyId = orderEntity.CompanyId,
                           OrderCount = orderEntity.OrderCount,
                           OrderStatus = orderEntity.OrderStatus,
                           UnitPrice = orderEntity.UnitPrice,
                           CreatedDate = orderEntity.CreatedDate,
                           LastUpdateDate = orderEntity.LastUpdateDate
                       });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedApiResponse.Data.Id, result.Data.Id);
            Assert.Equal(expectedApiResponse.Data.Name, result.Data.Name);
            Assert.Equal(expectedApiResponse.Data.UserId, result.Data.UserId);
            Assert.Equal(expectedApiResponse.Data.CompanyId, result.Data.CompanyId);
            Assert.Equal(expectedApiResponse.Data.OrderCount, result.Data.OrderCount);
            Assert.Equal(expectedApiResponse.Data.OrderStatus, result.Data.OrderStatus);
            Assert.Equal(expectedApiResponse.Data.UnitPrice, result.Data.UnitPrice);
            Assert.Equal(expectedApiResponse.Data.CreatedDate, result.Data.CreatedDate);
            Assert.Equal(expectedApiResponse.Data.LastUpdateDate, result.Data.LastUpdateDate);
        }

        [Fact]
        public async Task Handle_ReturnsFailWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var query = new GetOrderByIdQuery(orderId.ToString());

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync((Order)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            //Assert.Equal("Order not found", result.ErrorDto.Message);
        }
    }
}
