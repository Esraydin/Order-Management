using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;

public class UpdateProductCategoryCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
}
