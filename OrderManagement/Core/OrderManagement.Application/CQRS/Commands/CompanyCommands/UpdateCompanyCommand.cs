using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.CompanyCommands;

public class UpdateCompanyCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
