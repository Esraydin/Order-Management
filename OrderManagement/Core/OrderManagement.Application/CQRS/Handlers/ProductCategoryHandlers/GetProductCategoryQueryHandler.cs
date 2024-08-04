using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.ProductCategoryQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.Collections.Generic;

namespace ProductCategoryManagement.Application.CQRS.Handlers.ProductCategoryHandlers;

public class GetProductCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, ApiResponse<List<GetProductCategoryQueryResult>>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cachingService;
    public GetProductCategoryQueryHandler(IProductCategoryRepository repository, IMapper mapper, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<List<GetProductCategoryQueryResult>>> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "ProductCategoryList";

        // Cache'den veriyi al
        var cachedCompanies = await _cachingService.GetAsync<List<ProductCategory>>(cacheKey);

        if (cachedCompanies != null)
        {
            var cachedResult = _mapper.Map<List<GetProductCategoryQueryResult>>(cachedCompanies);
            return ApiResponse<List<GetProductCategoryQueryResult>>.Success(cachedResult);
        }

        // Cache'de yoksa veritabanından al
        var values = await _repository.GetAllAsync();

        // Veritabanında veri bulunamaması durumu
        if (values == null || !values.Any())
        {
            return ApiResponse<List<GetProductCategoryQueryResult>>.Fail("No Product Category found", ResponseType.Fail);
        }

        // Cache'e ekle
        await _cachingService.SetAsync(cacheKey, values, TimeSpan.FromMinutes(30));

        var result = _mapper.Map<List<GetProductCategoryQueryResult>>(values);

        return ApiResponse<List<GetProductCategoryQueryResult>>.Success(result);
    }
}

