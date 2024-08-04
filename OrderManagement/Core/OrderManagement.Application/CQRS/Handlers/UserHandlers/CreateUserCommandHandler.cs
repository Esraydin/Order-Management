using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Security.Hashing;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,ApiResponse<NoContent>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        
    }

    public async Task<ApiResponse<NoContent>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //await _userRepository.AddAsync(new User()
        //{
        //    Description = request.Description,
        //    Name = request.Name,
        //});

        //var result = _validator.Validate(request);
        //if (!result.IsValid)
        //{
        //    var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        //    throw new ValidationException(string.Join(", ", errorMessages));
        //}
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(request.Password,out passwordHash,out passwordSalt);
       
        var mappedUser = _mapper.Map<User>(request);
        mappedUser.PasswordHash = passwordHash;
        mappedUser.PasswordSalt = passwordSalt;
        await _userRepository.AddAsync(mappedUser);

        await _unitOfWork.SaveChangesAsync();
       
        return ApiResponse<NoContent>.Success(ResponseType.Success);
    }
}
