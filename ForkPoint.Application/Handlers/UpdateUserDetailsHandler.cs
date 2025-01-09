using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class UpdateUserDetailsHandler(
    ILogger<UpdateUserDetailsHandler> logger,
    IUserContext userContext,
    UserManager<User> userManager
)
    : IRequestHandler<UpdateUserDetailsRequest, UpdateUserDetailsResponse>
{
    public async Task<UpdateUserDetailsResponse> Handle(
        UpdateUserDetailsRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetCurrentUser() ?? throw new InvalidOperationException("User not authenticated");

        logger.LogInformation("Updating user details with {Request} for user {Email}", request, user.Email);

        var dbUser = await userManager.FindByIdAsync(user.Id) ??
                     throw new NotFoundException(nameof(User), user.Id);

        dbUser.FullName = request.FullName;

        var result = await userManager.UpdateAsync(dbUser);

        if (!result.Succeeded)
        {
            throw new Exception("Failed to update user");
        }

        return new UpdateUserDetailsResponse
        {
            IsSuccess = true,
            Message = "User details updated successfully"
        };
    }
}