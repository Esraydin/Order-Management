using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.CQRS.Handlers.UserHandlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.UserHandlers
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateUserCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUser_CreateUser()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "John Doe",
                // Add other properties as needed
            };

            _mapperMock.Setup(m => m.Map<User>(command))
                       .Returns(new User // Simulate mapping the command to a user entity
                       {
                           Name = command.Name,
                           // Map other properties as needed
                       });

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        // Add more test methods to cover edge cases, validation failures, etc.
    }
}
