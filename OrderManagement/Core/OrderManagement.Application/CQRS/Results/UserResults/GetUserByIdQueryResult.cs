namespace OrderManagement.Application.CQRS.Results.UserResults;

public class GetUserByIdQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;
}
