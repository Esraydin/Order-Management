using Moq;
using OrderManagement.Application.CQRS.Handlers.UserHandlers;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.UserHandlers
{
    public class RemoveUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RemoveUserCommandHandler _handler;

        public RemoveUserCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RemoveUserCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUserId_RemovesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToRemove = new User
            {
                Id = userId,
                Name = "John Doe",
                Description = "User 1"
                // Add other properties as needed
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
                           .ReturnsAsync(userToRemove);

            // Act
            var result = await _handler.Handle(new RemoveUserCommand (userId.ToString()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(userId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(userToRemove), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Assuming user with ID does not exist

            _repositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
                           .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(new RemoveUserCommand(userId.ToString()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(userId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }


        // Add more test methods to cover edge cases, such as handling exceptions, etc.
    }
}
