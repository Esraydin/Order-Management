using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
}
