using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.CompanyCommands;

public class CreateCompanyCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
