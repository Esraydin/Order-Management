using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Results.ProductCategoryResults;

public class GetProductCategoryQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;

}
