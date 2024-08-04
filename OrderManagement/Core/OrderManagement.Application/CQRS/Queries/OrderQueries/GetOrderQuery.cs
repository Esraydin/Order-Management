using MediatR;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.OrderQueries;

public class GetOrderQuery : IRequest<ApiResponse<List<GetOrderQueryResult>>>
{
}
