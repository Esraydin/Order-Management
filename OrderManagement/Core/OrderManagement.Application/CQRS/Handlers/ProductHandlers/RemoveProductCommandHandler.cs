using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductManagement.Application.CQRS.Handlers.ProductHandlers;

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, ApiResponse<NoContent>>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<NoContent>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id.ToString());

        if (product is null)
        {
            return ApiResponse<NoContent>.Fail("Product not found", ResponseType.Fail);
        }

        await _repository.RemoveAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}

