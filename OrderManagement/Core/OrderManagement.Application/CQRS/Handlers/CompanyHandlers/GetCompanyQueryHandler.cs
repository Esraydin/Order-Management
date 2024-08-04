using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.CompanyHandlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, ApiResponse<List<GetCompanyQueryResult>>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cachingService;

    public GetCompanyQueryHandler(ICompanyRepository repository, IMapper mapper, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<List<GetCompanyQueryResult>>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {

        //var companies = values.Select(x => new GetCompanyQueryResult()
        //{
        //    Id = x.Id,
        //    Name = x.Name,
        //    UserId = x.UserId,
        //    CreatedDate = x.CreatedDate,
        //    Description = x.Description,
        //    LastUpdateDate = x.LastUpdateDate,
        //}).ToList();
        var cacheKey = "CompanyList";

        // Cache'den veriyi al
        var cachedCompanies = await _cachingService.GetAsync<List<Company>>(cacheKey);

        if (cachedCompanies != null)
        {
            var cachedResult = _mapper.Map<List<GetCompanyQueryResult>>(cachedCompanies);
            return ApiResponse<List<GetCompanyQueryResult>>.Success(cachedResult);
        }

        // Cache'de yoksa veritabanından al
        var values = await _repository.GetAllAsync();

        // Veritabanında veri bulunamaması durumu
        if (values == null || !values.Any())
        {
            return ApiResponse<List<GetCompanyQueryResult>>.Fail("No companies found", ResponseType.Fail);
        }

        // Cache'e ekle
        await _cachingService.SetAsync(cacheKey, values, TimeSpan.FromMinutes(30));

        var result = _mapper.Map<List<GetCompanyQueryResult>>(values);

        return ApiResponse<List<GetCompanyQueryResult>>.Success(result);
    }
}
