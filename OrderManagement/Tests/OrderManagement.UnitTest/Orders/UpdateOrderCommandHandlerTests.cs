using AutoMapper;
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
    public class UpdateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapper;
        private readonly UpdateOrderCommandHandler _handler;

        public UpdateOrderCommandHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();

            _handler = new UpdateOrderCommandHandler(_repositoryMock.Object, _mapper.Object, _unitOfWorkMock.Object);
        }
        [Fact]
        public async Task Handle_ValidOrder_ReturnsSuccessResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var updatedOrderCommand = new UpdateOrderCommand
            {
                Id = orderId,
                Name = "Updated Order",
                UserId = Guid.NewGuid(),
                OrderStatus = OrderStatusEnum.Successfully,
                UnitPrice = 30.0m
            };

            var existingOrder = new Order
            {
                Id = orderId,
                Name = "Original Order",
                UserId = Guid.NewGuid(),
                OrderStatus = OrderStatusEnum.Pending,
                UnitPrice = 20.0m
            };

            // Set up repository mock to return the existing order
            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync(existingOrder);

            // Set up mapper mock to return the updated order
            _mapper.Setup(m => m.Map<Order>(It.Is<UpdateOrderCommand>(c =>
                c.Id == orderId &&
                c.Name == updatedOrderCommand.Name &&
                c.UserId == updatedOrderCommand.UserId &&
                c.OrderStatus == updatedOrderCommand.OrderStatus &&
                c.UnitPrice == updatedOrderCommand.UnitPrice
            )))
            .Returns(new Order
            {
                Id = orderId,
                Name = updatedOrderCommand.Name,
                UserId = updatedOrderCommand.UserId,
                OrderStatus = updatedOrderCommand.OrderStatus,
                UnitPrice = updatedOrderCommand.UnitPrice,
                CreatedDate = existingOrder.CreatedDate,
                LastUpdateDate = DateTime.UtcNow
            });

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(updatedOrderCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(orderId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Order>(o =>
                o.Id == orderId &&
                o.Name == updatedOrderCommand.Name &&
                o.UserId == updatedOrderCommand.UserId &&
                o.OrderStatus == updatedOrderCommand.OrderStatus &&
                o.UnitPrice == updatedOrderCommand.UnitPrice
            )), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidOrder_ReturnsFailResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId.ToString()))
                           .ReturnsAsync((Order)null); // Simulate returning null for non-existent order

            // Act
            var result = await _handler.Handle(new UpdateOrderCommand { Id = orderId }, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            // Verify that GetByIdAsync was called
            _repositoryMock.Verify(r => r.GetByIdAsync(orderId.ToString()), Times.Once);

            // Verify that UpdateAsync and SaveChanges methods were not called
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

    }
}
