using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.CompanyHandlers;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, ApiResponse<NoContent>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public UpdateCompanyCommandHandler(ICompanyRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        //await _repository.UpdateAsync(new Company()
        //{
        //    Id = request.Id,
        //    Description = request.Description,
        //    Name = request.Name,
        //    UserId = request.UserId,
        //    LastUpdateDate = DateTime.Now,
        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        var company = await _repository.GetByIdAsync(request.Id.ToString());

        if (company == null)
        {
            return ApiResponse<NoContent>.Fail("Company not found", ResponseType.Fail);
        }

        // Şirket bilgilerini güncelle
        _mapper.Map(request, company);

        await _repository.UpdateAsync(company);
        await _unitOfWork.SaveChangesAsync();

        // Cache anahtarını belirle
        var cacheKey = $"Company_{request.Id}";

        // Cache'i güncelle
        await _cachingService.RemoveAsync(cacheKey);
        await _cachingService.SetAsync(cacheKey, company, TimeSpan.FromMinutes(30));

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
