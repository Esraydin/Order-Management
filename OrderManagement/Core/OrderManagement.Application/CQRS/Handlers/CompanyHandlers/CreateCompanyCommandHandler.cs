using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.CQRS.Handlers.CompanyHandlers;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, ApiResponse<NoContent>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public CreateCompanyCommandHandler(ICompanyRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        //await _repository.AddAsync(new Company()
        //{
        //    Name = request.Name,
        //    Description = request.Description,
        //    UserId = request.UserId,
        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    List<ValidationError> errors = new List<ValidationError>();
        //    foreach (var item in result.Errors)
        //    {
        //        errors.Add(new ValidationError
        //        {
        //            Property = item.PropertyName,
        //            Message = item.ErrorMessage,
        //            Code = item.ErrorCode
        //        });

        //    }
        //    return ApiResponse<NoContent>.ValidatorErr(errors, ResponseType.ValidationError);
        //var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //throw new ValidationException(string.Join(", ", errorMessages));
        //}

        var company = _mapper.Map<Company>(request);

        await _repository.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();


        await _cachingService.RemoveAsync("CompanyList");

        // Cache süresini belirleyin, örneğin 1 saat
        var cacheExpiration = TimeSpan.FromHours(1);
        await _cachingService.SetAsync($"Company_{company.Id}", company, cacheExpiration);


        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
