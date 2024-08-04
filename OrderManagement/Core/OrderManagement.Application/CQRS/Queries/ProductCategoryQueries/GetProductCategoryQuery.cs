using MediatR;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.ProductCategoryQueries;

public class GetProductCategoryQuery : IRequest<ApiResponse<List<GetProductCategoryQueryResult>>>
{
}
