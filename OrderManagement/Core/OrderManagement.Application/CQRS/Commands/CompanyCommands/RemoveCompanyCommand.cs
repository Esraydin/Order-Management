using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.CompanyCommands;

public class RemoveCompanyCommand : IRequest<ApiResponse<NoContent>>
{
    public RemoveCompanyCommand(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}
