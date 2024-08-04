using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using OrderManagement.Application.CQRS.Results.CompanyResults;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.CompanyHandlers;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, ApiResponse<GetCompanyByIdQueryResult>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cacheService;
    public GetCompanyByIdQueryHandler(ICompanyRepository repository, IMapper mapper, ICachingService cacheService)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<GetCompanyByIdQueryResult>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        //var value = await _repository.GetByIdAsync(request.Id.ToString());
        //var company = new GetCompanyByIdQueryResult()
        //{
        //    Id = value.Id,
        //    Name = value.Name,
        //    UserId = value.UserId,
        //    Description = value.Description,
        //    CreatedDate = value.CreatedDate,
        //    LastUpdateDate = value.LastUpdateDate,
        //};

        var cacheKey = $"Company_{request.Id}";

        // Cache'den veriyi al
        var cachedCompany = await _cacheService.GetAsync<Company>(cacheKey);

        if (cachedCompany != null)
        {
            var cachedResult = _mapper.Map<GetCompanyByIdQueryResult>(cachedCompany);
            return ApiResponse<GetCompanyByIdQueryResult>.Success(cachedResult);
        }

        // Cache'de yoksa veritabanından al
        var value = await _repository.GetByIdAsync(request.Id.ToString());

        if (value != null)
        {
            // Cache'e ekle
            await _cacheService.SetAsync(cacheKey, value, TimeSpan.FromMinutes(30));
        }
        else
        {
            return ApiResponse<GetCompanyByIdQueryResult>.Fail("Company not found", ResponseType.Fail);
        }

        var newValue = _mapper.Map<GetCompanyByIdQueryResult>(value);

        return value != null
            ? ApiResponse<GetCompanyByIdQueryResult>.Success(_mapper.Map<GetCompanyByIdQueryResult>(value))
            : ApiResponse<GetCompanyByIdQueryResult>.Fail(new ErrorDto("", false), ResponseType.Fail);
    }
}
