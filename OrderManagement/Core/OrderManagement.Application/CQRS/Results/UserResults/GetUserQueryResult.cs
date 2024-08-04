using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Results.UserResults;

public class GetUserQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }
}
