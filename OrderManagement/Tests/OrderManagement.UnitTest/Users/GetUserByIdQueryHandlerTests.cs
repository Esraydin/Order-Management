using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Handlers.UserHandlers;
using OrderManagement.Application.CQRS.Queries.UserQueries;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.UserHandlers
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetUserByIdQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUserId_ReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Example user ID
            var query = new GetUserByIdQuery (userId.ToString());

            var userFromRepository = new User
            {
                Id = userId,
                Name = "John Doe",
                Description = "Test user",
                CreatedDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
                // Add other properties as needed
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
                           .ReturnsAsync(userFromRepository);

            _mapperMock.Setup(m => m.Map<GetUserByIdQueryResult>(userFromRepository))
                       .Returns(new GetUserByIdQueryResult // Simulate mapping to result
                       {
                           Id = userFromRepository.Id,
                           Name = userFromRepository.Name,
                           Description = userFromRepository.Description,
                           CreatedDate = userFromRepository.CreatedDate,
                           LastUpdateDate = userFromRepository.LastUpdateDate
                           // Map other properties as needed
                       });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);
            Assert.Equal(userId, result.Data.Id); // Verify user ID

            _repositoryMock.Verify(r => r.GetByIdAsync(userId.ToString()), Times.Once);
            _mapperMock.Verify(m => m.Map<GetUserByIdQueryResult>(userFromRepository), Times.Once);
        }

        // Add more test methods to cover edge cases, such as user not found, etc.
    }
}
