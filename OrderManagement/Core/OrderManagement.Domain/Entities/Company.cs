namespace OrderManagement.Domain.Entities;

public class Company : BaseEntity
{
    public Company()
    {
        Products = new List<Product>();
        Orders = new List<Order>();
    }

    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<Product> Products { get; set; }
    public List<Order> Orders { get; set; }
}
