using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Results.ProductResults;

public class GetProductQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;
    public int StockCount { get; set; }
    public decimal Price { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ProductCategoryId { get; set; }
}
