using MediatR;
using OrderManagement.Application.Security.JWT;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.Security.Dtos
{
    public class UserForLoginDto : IRequest<ApiResponse<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? AuthenticatorCode { get; set; }
    }
}
