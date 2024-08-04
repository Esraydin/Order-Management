using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductCategoryManagement.Application.CQRS.Handlers.ProductCategoryHandlers;

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand,ApiResponse<NoContent>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public CreateProductCategoryCommandHandler(IProductCategoryRepository repository, IMapper mapper,  IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {

        //await _repository.AddAsync(new ProductCategory()
        //{
        //   Name = request.Name,
        //   Description = request.Description,

        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        var productCategory = _mapper.Map<ProductCategory>(request);

        await _repository.AddAsync(productCategory);
        await _unitOfWork.SaveChangesAsync();


        await _cachingService.RemoveAsync("ProductCategoryList");

        // Cache süresini belirleyin, örneğin 1 saat
        var cacheExpiration = TimeSpan.FromHours(1);
        await _cachingService.SetAsync($"ProductCategory_{productCategory.Id}", productCategory, cacheExpiration);


        return ApiResponse<NoContent>.Success(ResponseType.Success);

        

    }
}
