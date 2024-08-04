namespace OrderManagement.Application.CQRS.Results.CompanyResults;

public class GetCompanyQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
