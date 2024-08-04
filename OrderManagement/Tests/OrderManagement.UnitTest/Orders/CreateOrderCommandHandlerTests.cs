using AutoMapper;
using Moq;
using OrderCategoryManagement.Application.CQRS.Handlers.OrderHandlers;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Handlers.OrderHandlers;
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
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateOrderCommandHandler(_repositoryMock.Object, _mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Name = "Test Order",
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                OrderCount = 5,
                OrderStatus = OrderStatusEnum.Pending,
                ProductId = Guid.NewGuid(),
                UnitPrice = 10.0m
            };

            _mapperMock.Setup(m => m.Map<Order>(command))
                       .Returns(new Order
                       {
                           Id = Guid.NewGuid(), // Mock a new Order with generated ID
                           Name = command.Name,
                           UserId = command.UserId,
                           CompanyId = command.CompanyId,
                           OrderCount = command.OrderCount,
                           OrderStatus = command.OrderStatus,
                           ProductId = command.ProductId,
                           UnitPrice = command.UnitPrice,
                           CreatedDate = DateTime.UtcNow,
                           LastUpdateDate = DateTime.UtcNow
                       });

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .Returns(Task.FromResult(true)); // Mock successful save operation

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Optionally, you can check if certain methods were called
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
