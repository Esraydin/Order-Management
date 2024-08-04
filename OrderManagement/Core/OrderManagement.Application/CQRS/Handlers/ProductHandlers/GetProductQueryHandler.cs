using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using ProductManagement.Application.CQRS.Queries.ProductQueries;
using System.Collections.Generic;

namespace ProductManagement.Application.CQRS.Handlers.ProductHandlers;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ApiResponse<List<GetProductQueryResult>>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    

    public GetProductQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
       
    }

    public async Task<ApiResponse<List<GetProductQueryResult>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var values = await _repository.GetAllAsync();
        //var products = values.Select(x => new GetProductQueryResult()
        //{
        //    Id = x.Id,
        //    Name = x.Name,
        //    Description = x.Description,
        //    Price = x.Price,
        //    ProductCategoryId = x.ProductCategoryId,
        //    StockCount = x.StockCount,
        //    CreatedDate = x.CreatedDate,
        //    CompanyId = x.CompanyId,
        //    LastUpdateDate = x.LastUpdateDate,
        //}).ToList();

        return ApiResponse<List<GetProductQueryResult>>.Success(_mapper.Map<List<GetProductQueryResult>>(values));
    }
}
