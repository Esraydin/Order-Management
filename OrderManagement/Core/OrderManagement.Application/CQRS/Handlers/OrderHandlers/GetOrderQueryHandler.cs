using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.OrderQueries;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Collections.Generic;

namespace OrderManagement.Application.CQRS.Handlers.OrderHandlers;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, ApiResponse<List<GetOrderQueryResult>>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    

    public GetOrderQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<ApiResponse<List<GetOrderQueryResult>>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var values = await _repository.GetAllAsync();
        //var companies = values.Select(x => new GetOrderQueryResult()
        //{
        //    Id = x.Id,
        //    Name = x.Name,
        //    UserId = x.UserId,
        //    CreatedDate = x.CreatedDate,
        //    CompanyId = x.CompanyId,
        //    OrderCount = x.OrderCount,
        //    OrderStatus = x.OrderStatus,
        //    ProductId = x.ProductId,
        //    UnitPrice = x.UnitPrice,
        //    LastUpdateDate = x.LastUpdateDate,
        //}).ToList();

        //return companies;
        

        return ApiResponse<List<GetOrderQueryResult>>.Success(_mapper.Map<List<GetOrderQueryResult>>(values));
    }
}
