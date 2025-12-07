using System.IdentityModel.Tokens.Jwt;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Handlers;

public class LoginHandler(
    ILogger<LoginHandler> logger,
    IAuthService authService,
    IUserRepository userRepository,
    SignInManager<User> signInManager
) : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(request.Email) ??
                   throw new NotFoundException(nameof(User), request.Email);

        if (!user.EmailConfirmed)
        {
            logger.LogError("Failed to login user with email {Email}", request.Email);
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Please confirm your email address first."
            };
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to login user with email {Email}. Wrong Password", request.Email);
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "The email and password combination provided is not correct. Try again."
            };
        }

        var token = await authService.GenerateAccessToken(user);
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
        var refreshToken = await authService.GenerateRefreshToken(user);

        return new LoginResponse(token, expiry)
        {
            IsSuccess = true
        };
    }
}