namespace OrderManagement.Application.CQRS.Results;

public class GetBaseQueryResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}
