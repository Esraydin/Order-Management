using MediatR;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Queries.CompanyQueries;

public class GetCompanyByIdQuery : IRequest<ApiResponse<GetCompanyByIdQueryResult>>
{
    public Guid Id { get; set; }
    public GetCompanyByIdQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}
