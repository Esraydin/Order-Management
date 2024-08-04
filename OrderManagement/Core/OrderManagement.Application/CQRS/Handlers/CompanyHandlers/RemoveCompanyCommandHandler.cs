using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.CompanyHandlers;

public class RemoveCompanyCommandHandler: IRequestHandler<RemoveCompanyCommand,ApiResponse<NoContent>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public RemoveCompanyCommandHandler(ICompanyRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(RemoveCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id.ToString());

        if (company == null)
        {
            return ApiResponse<NoContent>.Fail("Company not found", ResponseType.Fail);
        }

        // Şirketi sil
        await _repository.RemoveAsync(company);
        await _unitOfWork.SaveChangesAsync();

        // Cache'i güncelle
        var cacheKey = $"Company_{request.Id}";
        await _cachingService.RemoveAsync(cacheKey);
        await _cachingService.RemoveAsync("CompanyList");

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
