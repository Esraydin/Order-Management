using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Results.OrderResults;

public class GetOrderByIdQueryResult : GetBaseQueryResult
{
    public int OrderCount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * OrderCount;
    public OrderStatusEnum OrderStatus { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}
