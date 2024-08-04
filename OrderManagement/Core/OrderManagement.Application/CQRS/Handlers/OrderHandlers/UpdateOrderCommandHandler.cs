using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.OrderHandlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse<NoContent>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<NoContent>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id.ToString());

        if (order is null)
        {
            return ApiResponse<NoContent>.Fail("Order not found", ResponseType.Fail);
        }

        await _repository.UpdateAsync(_mapper.Map<Order>(request));
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}

