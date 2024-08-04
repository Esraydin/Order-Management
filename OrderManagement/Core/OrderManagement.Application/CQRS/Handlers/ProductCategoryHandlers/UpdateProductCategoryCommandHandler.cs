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

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand,ApiResponse<NoContent>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;
    public UpdateProductCategoryCommandHandler(IProductCategoryRepository repository, IMapper mapper,  IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        //await _repository.UpdateAsync(new ProductCategory()
        //{
        //    Id = request.Id,
        //    Description = request.Description,
        //    Name = request.Name,
        //    LastUpdateDate = DateTime.Now,

        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}


        var productCategory = await _repository.GetByIdAsync(request.Id.ToString());

        if (productCategory == null)
        {
            return ApiResponse<NoContent>.Fail("Product Category not found", ResponseType.Fail);
        }

        // Şirket bilgilerini güncelle
        _mapper.Map(request, productCategory);

        await _repository.UpdateAsync(productCategory);
        await _unitOfWork.SaveChangesAsync();

        // Cache anahtarını belirle
        var cacheKey = $"ProductCategory_{request.Id}";

        // Cache'i güncelle
        await _cachingService.RemoveAsync(cacheKey);
        await _cachingService.SetAsync(cacheKey, productCategory, TimeSpan.FromMinutes(30));

        return ApiResponse<NoContent>.Success(ResponseType.Success);



    }

   
}
