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
using AutoMapper;

namespace OrderManagement.UnitTest.UserHandlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateUserCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUserUpdate_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserCommand = new UpdateUserCommand
            {
                Id = userId,
                Name = "Updated Name",
                Description = "Updated Description"
                // Add other properties as needed
            };

            var existingUser = new User
            {
                Id = userId,
                Name = "Original Name",
                Description = "Original Description"
                // Add other properties as needed
            };

            // Setup the repository mock to return the existing user
            _repositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
                           .ReturnsAsync(existingUser);

            // Setup the mapper mock to map the command to the user entity
            _mapperMock.Setup(m => m.Map<User>(updateUserCommand))
                       .Returns(new User
                       {
                           Id = userId,
                           Name = "Updated Name",
                           Description = "Updated Description"
                           // Add other properties as needed
                       });

            // Act
            var result = await _handler.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            // Verify that GetByIdAsync was called
            _repositoryMock.Verify(r => r.GetByIdAsync(userId.ToString()), Times.Once);

            // Verify that UpdateAsync was called with the updated user
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.Id == userId &&
                u.Name == "Updated Name" &&
                u.Description == "Updated Description"
            )), Times.Once);

            // Verify that SaveChangesAsync was called
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Assuming user with ID 999 does not exist

            var updateUserCommand = new UpdateUserCommand
            {
                Id = userId,
                Name = "Updated Name",
                Description = "Updated Description"
                // Add other properties as needed
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
                           .ReturnsAsync((User)null);

            _mapperMock.Setup(m => m.Map<User>(updateUserCommand))
                       .Returns(new User
                       {
                           Id = userId,
                           Name = "Updated Name",
                           Description = "Updated Description"
                           // Add other properties as needed
                       });


            // Act
            var result = await _handler.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);

            _repositoryMock.Verify(r => r.GetByIdAsync(userId.ToString()), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        // Add more test methods to cover edge cases, such as handling exceptions, etc.
    }
}
