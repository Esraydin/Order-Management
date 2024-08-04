using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Results.ProductCategoryResults;

public class GetProductCategoryByIdQueryResult : GetBaseQueryResult
{
    public string Description { get; set; } = string.Empty;

}
