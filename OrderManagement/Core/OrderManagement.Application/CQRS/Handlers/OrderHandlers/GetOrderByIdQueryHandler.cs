using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Queries.OrderQueries;
using OrderManagement.Application.CQRS.Results.OrderResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.OrderHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResponse<GetOrderByIdQueryResult>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
   

    public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<ApiResponse<GetOrderByIdQueryResult>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var value = await _repository.GetByIdAsync(request.Id.ToString());
        //var Order = new GetOrderByIdQueryResult()
        //{
        //    Id = value.Id,
        //    Name = value.Name,
        //    UserId = value.UserId,
        //    CompanyId = value.CompanyId,
        //    OrderCount = value.OrderCount,
        //    OrderStatus = value.OrderStatus,
        //    UnitPrice = value.UnitPrice,
        //    CreatedDate = value.CreatedDate,
        //    LastUpdateDate = value.LastUpdateDate,
        //};

        //return Order;

        return value != null 
            ? ApiResponse<GetOrderByIdQueryResult>.Success(_mapper.Map<GetOrderByIdQueryResult>(value))
            : ApiResponse<GetOrderByIdQueryResult>.Fail(new ErrorDto("", false), ResponseType.Fail);
    }
}
