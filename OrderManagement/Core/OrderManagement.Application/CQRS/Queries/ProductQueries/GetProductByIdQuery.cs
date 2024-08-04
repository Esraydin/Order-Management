using MediatR;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductManagement.Application.CQRS.Queries.ProductQueries;

public class GetProductByIdQuery : IRequest<ApiResponse<GetProductByIdQueryResult>>
{
    public Guid Id { get; set; }
    public GetProductByIdQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}
