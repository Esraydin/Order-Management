using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.ProductCommands;

public class UpdateProductCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int StockCount { get; set; }
    public decimal Price { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ProductCategoryId { get; set; }
}
