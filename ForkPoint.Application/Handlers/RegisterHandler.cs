using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class RegisterHandler(
    ILogger<RegisterHandler> logger,
    UserManager<User> userManager
) : BaseHandler<RegisterRequest, RegisterResponse>
{
    public override async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user with email {Email}...", request.Email);

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
            logger.LogError("Failed to register user with email {Email}", request.Email);

            logger.LogError("Error: {Error}", errors);

            return new RegisterResponse
            {
                IsSuccess = false,
                Message =
                    $"Failed to register user. {errors}"
            };
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, roleResult.Errors.Select(e => e.Description));

            logger.LogError("Failed to assign role to user with email {Email}", request.Email);

            logger.LogError("Error: {Error}", errors);


            return new RegisterResponse
            {
                IsSuccess = false,
                Message =
                    $"Failed to assign role to user. {errors}"
            };
        }

        logger.LogInformation("User with email {Email} registered successfully", request.Email);

        return new RegisterResponse
        {
            IsSuccess = true,
            Message = $"User registered successfully - {request.Email}"
        };
    }
}