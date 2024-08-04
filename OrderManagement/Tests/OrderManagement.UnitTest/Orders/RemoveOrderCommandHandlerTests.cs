using Moq;
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
    public class RemoveOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RemoveOrderCommandHandler _handler;

        public RemoveOrderCommandHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RemoveOrderCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidOrder_ReturnsSuccessResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderToRemove = new Order
            {
                Id = orderId,
                Name = "Test Order",
                UserId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                OrderStatus = OrderStatusEnum.Pending,
                UnitPrice = 25.0m,
                LastUpdateDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync(orderToRemove);

            // Act
            var result = await _handler.Handle(new RemoveOrderCommand (orderId.ToString()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify that RemoveAsync and SaveChanges methods were called
            _repositoryMock.Verify(r => r.GetByIdAsync(orderId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(orderToRemove), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidOrder_ReturnsFailResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync((Order)null); // Simulate returning null for non-existent order

            // Act
            var result = await _handler.Handle(new RemoveOrderCommand(orderId.ToString()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.ErrorDto);

            // Verify that GetByIdAsync was called
            _repositoryMock.Verify(r => r.GetByIdAsync(orderId.ToString()), Times.Once);

            // Verify that RemoveAsync and SaveChanges methods were not called
            _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Order>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

    }
}
