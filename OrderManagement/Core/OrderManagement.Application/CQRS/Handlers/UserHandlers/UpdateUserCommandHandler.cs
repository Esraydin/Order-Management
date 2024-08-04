using AutoMapper;
using FluentValidation;
using MediatR;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.ProductCommands;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Security.Hashing;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,ApiResponse<NoContent>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    

    public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        
    }

    public async Task<ApiResponse<NoContent>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        //await _repository.UpdateAsync(new User()
        //{
        //    Id = request.Id,
        //    Description = request.Description,
        //    Name = request.Name,
        //    LastUpdateDate = DateTime.Now,
        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}

        HashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var mappedUser = _mapper.Map<User>(request);
        mappedUser.PasswordSalt = passwordSalt;
        mappedUser.PasswordHash = passwordHash;
        //await _repository.UpdateAsync(mappedUser);

        var user = await _repository.GetByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return ApiResponse<NoContent>.Fail("Order not found", ResponseType.Fail);
        }

        await _repository.UpdateAsync(_mapper.Map<User>(request));
        await _unitOfWork.SaveChangesAsync();
       
        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
