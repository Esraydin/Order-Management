using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderCategoryManagement.Application.CQRS.Handlers.OrderHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<NoContent>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;


    public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }



    public async Task<ApiResponse<NoContent>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //await _repository.AddAsync(new Order()
        //{
        //    Name = request.Name,
        //    UserId = request.UserId,
        //    CompanyId = request.CompanyId,
        //    OrderCount = request.OrderCount,
        //    OrderStatus = request.OrderStatus,
        //    ProductId = request.ProductId,
        //    UnitPrice = request.UnitPrice,


        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        await _repository.AddAsync(_mapper.Map<Order>(request));
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
