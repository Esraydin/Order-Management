using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        Orders = new List<Order>();
        Companies = new List<Company>();
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Order> Orders { get; set; }
    public List<Company> Companies { get; set; }
}
