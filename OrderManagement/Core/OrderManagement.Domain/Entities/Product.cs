namespace OrderManagement.Domain.Entities;

public class Product : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int StockCount { get; set; }
    public decimal Price { get; set; }
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public Guid ProductCategoryId { get; set; }
    public ProductCategory? ProductCategory { get; set; }

    public List<Order> Orders { get; set; }
}
