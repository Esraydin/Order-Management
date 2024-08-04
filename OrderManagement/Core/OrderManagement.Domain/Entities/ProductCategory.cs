namespace OrderManagement.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public ProductCategory()
    {
        Products = new List<Product>();
    }

    public string Description { get; set; } = string.Empty;
    public List<Product> Products { get; set; }
}
