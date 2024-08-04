using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.ProductCommands;

public class RemoveProductCommand : IRequest<ApiResponse<NoContent>>
{
    public RemoveProductCommand(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}
