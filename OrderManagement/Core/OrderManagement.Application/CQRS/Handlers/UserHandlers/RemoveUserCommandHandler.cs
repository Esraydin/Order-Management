using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, ApiResponse<NoContent>>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<NoContent>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return ApiResponse<NoContent>.Fail("User not found", ResponseType.Fail);
        }
        user.Status = false;

        await _repository.UpdateAsync(user);

        await _repository.RemoveAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}

