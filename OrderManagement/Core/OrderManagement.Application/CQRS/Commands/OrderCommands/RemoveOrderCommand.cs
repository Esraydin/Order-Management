using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.OrderCommands;

public class RemoveOrderCommand : IRequest<ApiResponse<NoContent>>
{
    public RemoveOrderCommand(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}
