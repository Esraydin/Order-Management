using AutoMapper;
using MediatR;
using OrderManagement.Application.CQRS.Commands.UserCommands;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Security.Dtos;
using OrderManagement.Application.Security.JWT;
using OrderManagement.Domain.Entities;
using OrderManagement.SharedLayer.Enums;
using OrderManagement.SharedLayer.ResponseModel;

namespace OrderManagement.Application.CQRS.Handlers.UserHandlers;

public class LoginCommandHandler : IRequestHandler<UserForLoginDto, ApiResponse<AccessToken>>
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(ITokenHelper tokenHelper, IUserRepository userRepository)
    {
        _tokenHelper = tokenHelper;
        _userRepository = userRepository;
    }


    public async Task<ApiResponse<AccessToken>> Handle(UserForLoginDto request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<AccessToken>.Fail("User not found.",ResponseType.Fail);
        }


        var token = _tokenHelper.CreateToken(user);

        return ApiResponse<AccessToken>.Success(token);
    }
}

