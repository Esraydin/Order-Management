using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Commands.ProductCategoryCommands;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductManagement.Application.CQRS.Handlers.ProductHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand,ApiResponse<NoContent>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    


    public CreateProductCommandHandler(IProductRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        
    }

    public async Task<ApiResponse<NoContent>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //await _repository.AddAsync(new Product()
        //{
        //    Name = request.Name,
        //    CompanyId = request.CompanyId,
        //    Description = request.Description,
        //    Price = request.Price,
        //    StockCount = request.StockCount,
        //    ProductCategoryId = request.ProductCategoryId,

        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        await _repository.AddAsync(_mapper.Map<Product>(request));
        await _unitOfWork.SaveChangesAsync();
       
        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
