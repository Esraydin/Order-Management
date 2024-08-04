using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Handlers.OrderHandlers;
using OrderManagement.Application.CQRS.Queries.OrderQueries;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.SharedLayer.Enums;

namespace OrderManagement.UnitTest.OrderHandlers
{
    public class GetOrderQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderQueryHandler _handler;

        public GetOrderQueryHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOrderQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessResponseWithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Name = "Order 1",
                    UserId = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    CompanyId = Guid.NewGuid(),
                    OrderCount = 2,
                    OrderStatus = OrderStatusEnum.Pending,
                    ProductId = Guid.NewGuid(),
                    UnitPrice = 15.5m,
                    LastUpdateDate = DateTime.UtcNow
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    Name = "Order 2",
                    UserId = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    CompanyId = Guid.NewGuid(),
                    OrderCount = 1,
                    OrderStatus = OrderStatusEnum.Successfully,
                    ProductId = Guid.NewGuid(),
                    UnitPrice = 20.0m,
                    LastUpdateDate = DateTime.UtcNow
                }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(orders);

            _mapperMock.Setup(m => m.Map<List<GetOrderQueryResult>>(orders))
                       .Returns(new List<GetOrderQueryResult>
                       {
                           new GetOrderQueryResult
                           {
                               Id = orders[0].Id,
                               Name = orders[0].Name,
                               UserId = orders[0].UserId,
                               CreatedDate = orders[0].CreatedDate,
                               CompanyId = orders[0].CompanyId,
                               OrderCount = orders[0].OrderCount,
                               OrderStatus = orders[0].OrderStatus,
                               ProductId = orders[0].ProductId,
                               UnitPrice = orders[0].UnitPrice,
                               LastUpdateDate = orders[0].LastUpdateDate
                           },
                           new GetOrderQueryResult
                           {
                               Id = orders[1].Id,
                               Name = orders[1].Name,
                               UserId = orders[1].UserId,
                               CreatedDate = orders[1].CreatedDate,
                               CompanyId = orders[1].CompanyId,
                               OrderCount = orders[1].OrderCount,
                               OrderStatus = orders[1].OrderStatus,
                               ProductId = orders[1].ProductId,
                               UnitPrice = orders[1].UnitPrice,
                               LastUpdateDate = orders[1].LastUpdateDate
                           }
                       });

            var query = new GetOrderQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count); // Verify that two orders are returned

            // Optionally, you can check the specific order details returned
            Assert.Equal(orders[0].Id, result.Data[0].Id);
            Assert.Equal(orders[0].Name, result.Data[0].Name);
            Assert.Equal(orders[1].Id, result.Data[1].Id);
            Assert.Equal(orders[1].Name, result.Data[1].Name);

            // Optionally, verify if repository method was called
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
