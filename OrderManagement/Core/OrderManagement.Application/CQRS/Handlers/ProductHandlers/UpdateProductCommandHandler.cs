using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.OrderCommands;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace ProductManagement.Application.CQRS.Handlers.ProductHandlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand,ApiResponse<NoContent>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
  

    public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        
    }

    public async Task<ApiResponse<NoContent>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        //await _repository.UpdateAsync(new Product()
        //{
        //    Id = request.Id,
        //    CompanyId = request.CompanyId,
        //    Description = request.Description,
        //    Name = request.Name,
        //    Price = request.Price,
        //    ProductCategoryId = request.ProductCategoryId,
        //    StockCount = request.StockCount,
        //    LastUpdateDate = DateTime.Now,
        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        var product = await _repository.GetByIdAsync(request.Id.ToString());

        if (product is null)
        {
            return ApiResponse<NoContent>.Fail("Product not found", ResponseType.Fail);
        }

        await _repository.UpdateAsync(_mapper.Map<Product>(request));
        await _unitOfWork.SaveChangesAsync();
        
        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
