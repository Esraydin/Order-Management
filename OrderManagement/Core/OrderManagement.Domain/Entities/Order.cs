using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderStatus = OrderStatusEnum.Successfully;
    }

    public int OrderCount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * OrderCount;
    public OrderStatusEnum OrderStatus { get; set; }
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
