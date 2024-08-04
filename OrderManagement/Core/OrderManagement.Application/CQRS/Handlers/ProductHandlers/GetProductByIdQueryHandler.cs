using AutoMapper;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Results.ProductCategoryResults;
using OrderManagement.Application.CQRS.Results.ProductResults;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;
using ProductManagement.Application.CQRS.Queries.ProductQueries;

namespace ProductManagement.Application.CQRS.Handlers.ProductHandlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<GetProductByIdQueryResult>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    
    public GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<ApiResponse<GetProductByIdQueryResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var value = await _repository.GetByIdAsync(request.Id.ToString());
        //var Product = new GetProductByIdQueryResult()
        //{
        //    Id = value.Id,
        //    Name = value.Name,
        //    CompanyId = value.CompanyId,
        //    Description = value.Description,
        //    Price = value.Price,
        //    StockCount = value.StockCount,
        //    ProductCategoryId = value.ProductCategoryId,
        //    CreatedDate = value.CreatedDate,
        //    LastUpdateDate = value.LastUpdateDate,
        //};



        return value != null
            ? ApiResponse<GetProductByIdQueryResult>.Success(_mapper.Map<GetProductByIdQueryResult>(value))
            : ApiResponse<GetProductByIdQueryResult>.Fail(new ErrorDto("", false), ResponseType.Fail);
    }
}
