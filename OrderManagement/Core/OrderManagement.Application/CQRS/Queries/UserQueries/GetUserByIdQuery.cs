using MediatR;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.UserQueries;

public class GetUserByIdQuery : IRequest<ApiResponse<GetUserByIdQueryResult>>
{
    public Guid Id { get; set; }
    public GetUserByIdQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}
