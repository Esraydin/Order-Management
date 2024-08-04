using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.OrderQueries;
using OrderManagement.Application.CQRS.Queries.ProductCategoryQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.ProductCategoryHandlers;

public class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, ApiResponse<GetProductCategoryByIdQueryResult>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cachingService;
    public GetProductCategoryByIdQueryHandler(IProductCategoryRepository repository, IMapper mapper, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<GetProductCategoryByIdQueryResult>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {

        //var productCategory = new GetProductCategoryByIdQueryResult()
        //{
        //   Description=value.Description,
        //   Name=value.Name,
        //   CreatedDate=value.CreatedDate,
        //   Id=value.Id,
        //   LastUpdateDate = value.LastUpdateDate

        //};

        //return productCategory;
        // Cache'i güncelle

        var cacheKey = $"ProductCategory_{request.Id}";

        // Cache'den veriyi al
        var cachedCompany = await _cachingService.GetAsync<ProductCategory>(cacheKey);

        if (cachedCompany != null)
        {
            var cachedResult = _mapper.Map<GetProductCategoryByIdQueryResult>(cachedCompany);
            return ApiResponse<GetProductCategoryByIdQueryResult>.Success(cachedResult);
        }

        // Cache'de yoksa veritabanından al
        var value = await _repository.GetByIdAsync(request.Id.ToString());

        if (value != null)
        {
            // Cache'e ekle
            await _cachingService.SetAsync(cacheKey, value, TimeSpan.FromMinutes(30));
        }
        else
        {
            return ApiResponse<GetProductCategoryByIdQueryResult>.Fail("Product Category not found", ResponseType.Fail);
        }

        var newValue = _mapper.Map<GetProductCategoryByIdQueryResult>(value);

        return value != null
            ? ApiResponse<GetProductCategoryByIdQueryResult>.Success(_mapper.Map<GetProductCategoryByIdQueryResult>(value))
            : ApiResponse<GetProductCategoryByIdQueryResult>.Fail(new ErrorDto("", false), ResponseType.Fail);

    }
}
