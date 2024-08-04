namespace OrderManagement.Domain.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
        LastUpdateDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get;  set; }
    public DateTime LastUpdateDate { get; set; }
}
