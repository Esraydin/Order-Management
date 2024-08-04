using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.UserCommands;

public class UpdateUserCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
}
