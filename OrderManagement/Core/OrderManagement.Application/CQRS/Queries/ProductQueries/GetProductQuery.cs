using MediatR;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductManagement.Application.CQRS.Queries.ProductQueries;

public class GetProductQuery : IRequest<ApiResponse<List<GetProductQueryResult>>>
{
}
