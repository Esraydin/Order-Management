using AutoMapper;
using Moq;
using OrderManagement.Application.CQRS.Handlers.UserHandlers;
using OrderManagement.Application.CQRS.Queries.UserQueries;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTest.UserHandlers
{
    public class GetUserQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserQueryHandler _handler;

        public GetUserQueryHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetUserQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnListOfUsers()
        {
            // Arrange
            var usersFromRepository = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "John Doe",
                    Description = "User 1",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    LastUpdateDate = DateTime.UtcNow.AddDays(-1)
                    // Add other properties as needed
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Smith",
                    Description = "User 2",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    LastUpdateDate = DateTime.UtcNow.AddDays(-2)
                    // Add other properties as needed
                }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(usersFromRepository);

            _mapperMock.Setup(m => m.Map<List<GetUserQueryResult>>(usersFromRepository))
                       .Returns(usersFromRepository.Select(u => new GetUserQueryResult
                       {
                           Id = u.Id,
                           Name = u.Name,
                           Description = u.Description,
                           CreatedDate = u.CreatedDate,
                           LastUpdateDate = u.LastUpdateDate
                           // Map other properties as needed
                       }).ToList());

            var query = new GetUserQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.NotNull(result.Data);
            Assert.Equal(usersFromRepository.Count, result.Data.Count); // Verify number of users returned

            foreach (var expectedUser in usersFromRepository)
            {
                var actualUser = result.Data.FirstOrDefault(u => u.Id == expectedUser.Id);
                Assert.NotNull(actualUser);
                Assert.Equal(expectedUser.Name, actualUser.Name);
                // Assert other properties as needed
            }

            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetUserQueryResult>>(usersFromRepository), Times.Once);
        }

        // Add more test methods to cover edge cases, such as empty list returned, etc.
    }
}
