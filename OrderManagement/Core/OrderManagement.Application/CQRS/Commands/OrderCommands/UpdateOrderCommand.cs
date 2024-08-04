using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.OrderCommands;

public class UpdateOrderCommand : BaseCommand, IRequest<ApiResponse<NoContent>>
{
    public Guid Id { get; set; }
    public int OrderCount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * OrderCount;
    public OrderStatusEnum OrderStatus { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}
