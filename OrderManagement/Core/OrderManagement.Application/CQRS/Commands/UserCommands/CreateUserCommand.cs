using AutoMapper.Configuration.Annotations;
using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.UserCommands;

public class CreateUserCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
    

}
