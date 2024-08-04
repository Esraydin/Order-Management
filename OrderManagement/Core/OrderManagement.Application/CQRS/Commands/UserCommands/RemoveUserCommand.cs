using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.UserCommands;

public class RemoveUserCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public RemoveUserCommand(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}
