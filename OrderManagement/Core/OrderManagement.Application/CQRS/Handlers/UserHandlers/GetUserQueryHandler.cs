using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Queries.UserQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.UserResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Collections.Generic;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApiResponse<List<GetUserQueryResult>>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    


    public GetUserQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<ApiResponse<List<GetUserQueryResult>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var values = await _repository.GetAllAsync();
        //var users = values.Select(x => new GetUserQueryResult()
        //{
        //    Id = x.Id,
        //    Name = x.Name,
        //    CreatedDate = x.CreatedDate,
        //    Description = x.Description,
        //    LastUpdateDate = x.LastUpdateDate,
        //}).ToList();


        return ApiResponse<List<GetUserQueryResult>>.Success(_mapper.Map<List<GetUserQueryResult>>(values));
    }
}
