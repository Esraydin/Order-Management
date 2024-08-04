using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Queries.UserQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdQueryResult>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
   
    public GetUserByIdQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<ApiResponse<GetUserByIdQueryResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var value = await _repository.GetByIdAsync(request.Id.ToString());
        //var user = new GetUserByIdQueryResult()
        //{
        //    Id = value.Id,
        //    Name = value.Name,
        //    Description = value.Description,
        //    CreatedDate = value.CreatedDate,
        //    LastUpdateDate = value.LastUpdateDate,
        //};


        return value != null
            ? ApiResponse<GetUserByIdQueryResult>.Success(_mapper.Map<GetUserByIdQueryResult>(value))
            : ApiResponse<GetUserByIdQueryResult>.Fail(new ErrorDto("", false), ResponseType.Fail);
    }
}
