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

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to login user with email {Email}", request.Email);
            return new LoginResponse(null)
            {
                IsSuccess = false,
                Message = "Invalid email or password."
            };
        }

        var token = authService.GenerateToken(user);

        return new LoginResponse(token)
        {
            IsSuccess = true
        };
    }
}