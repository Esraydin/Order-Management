using MediatR;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.ProductCategoryQueries;

public class GetProductCategoryByIdQuery : IRequest<ApiResponse<GetProductCategoryByIdQueryResult>>
{
    public Guid Id { get; set; }
    public GetProductCategoryByIdQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}
