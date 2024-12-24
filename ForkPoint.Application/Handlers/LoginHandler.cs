using System.IdentityModel.Tokens.Jwt;
using ForkPoint.Application.Constants;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class LoginHandler(
    ILogger<LoginHandler> logger,
    IAuthService authService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : BaseHandler<LoginRequest, LoginResponse>
{
    public override async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ??
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
                Message = "That email and password combination is not correct. Try again."
            };
        }

        var token = await authService.GenerateToken(user);
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
        var refreshToken = authService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(AuthConstants.RefreshTokenExpirationInHours);
        await userManager.UpdateAsync(user);

        return new LoginResponse(token, refreshToken, expiry)
        {
            IsSuccess = true
        };
    }
}