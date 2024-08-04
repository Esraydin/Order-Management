using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.OrderHandlers;

public class RemoveOrderCommandHandler : IRequestHandler<RemoveOrderCommand, ApiResponse<NoContent>>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;


    public RemoveOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;

    }

    public async Task<ApiResponse<NoContent>> Handle(RemoveOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id.ToString());

        if (order == null)
        {
            return ApiResponse<NoContent>.Fail("", ResponseType.Fail);
        }

        await _repository.RemoveAsync(order);
        _unitOfWork.SaveChanges();

        return order is null
            ? ApiResponse<NoContent>.Fail("", ResponseType.Fail)
            : ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
