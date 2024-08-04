using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Commands.ProductCategory;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.OrderHandlers;

public class RemoveProductCategoryCommandHandler : IRequestHandler<RemoveProductCategoryCommand,ApiResponse<NoContent>>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICachingService _cachingService;

    public RemoveProductCategoryCommandHandler(IProductCategoryRepository repository, IUnitOfWork unitOfWork, ICachingService cachingService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cachingService = cachingService;
    }

    public async Task<ApiResponse<NoContent>> Handle(RemoveProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _repository.GetByIdAsync(request.Id.ToString());

        if (productCategory == null)
        {
            return ApiResponse<NoContent>.Fail("Product Category not found", ResponseType.Fail);
        }

        // Şirketi sil
        await _repository.RemoveAsync(productCategory);
        await _unitOfWork.SaveChangesAsync();

        // Cache'i güncelle
        var cacheKey = $"ProductCategory_{request.Id}";
        await _cachingService.RemoveAsync(cacheKey);
        await _cachingService.RemoveAsync("ProductCategoryList");

        return ApiResponse<NoContent>.Success(ResponseType.Success);




        
    }
}
