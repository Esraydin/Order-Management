using MediatR;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.UserQueries;

public class GetUserQuery : IRequest<ApiResponse<List<GetUserQueryResult>>>
{
}
