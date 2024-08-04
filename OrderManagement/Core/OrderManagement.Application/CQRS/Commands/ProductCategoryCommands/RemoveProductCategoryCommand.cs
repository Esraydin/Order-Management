using MediatR;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Commands.ProductCategory;

public class RemoveProductCategoryCommand : IRequest<ApiResponse<NoContent>>
{
    public RemoveProductCategoryCommand(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}
