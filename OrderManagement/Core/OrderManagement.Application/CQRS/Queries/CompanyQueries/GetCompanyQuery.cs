using MediatR;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.CompanyQueries;

public class GetCompanyQuery : IRequest<ApiResponse<List<GetCompanyQueryResult>>>
{
}
