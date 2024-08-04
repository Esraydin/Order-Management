namespace OrderManagement.Application.CQRS.Results.CompanyResults;

public class GetCompanyByIdQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
