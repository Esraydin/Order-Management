using OrderManagement.Application.Security.Entities;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user);
        RefreshToken CreateRefreshToken(User user, string ipAddress);
    }
}
