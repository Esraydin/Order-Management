using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Persistence.Context;
using OrderManagement.Persistence.Repositories;

namespace OrderManagement.Persistence.Concrete;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }
}
