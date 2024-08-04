using MediatR;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.OrderQueries;

public class GetOrderByIdQuery : IRequest<ApiResponse<GetOrderByIdQueryResult>>
{
    public Guid Id { get; set; }
    public GetOrderByIdQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}
