using ForkPoint.Application.Contexts;
using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Infrastructure.Authorization.Handlers;

public class OwnsResourceOrAdminHandler(
    ILogger<OwnsResourceOrAdminHandler> logger,
    IUserContext userContext,
    UserManager<User> userManager
) : AuthorizationHandler<OwnsResourceOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnsResourceOrAdminRequirement requirement
    )
    {
        logger.LogInformation("Checking if user owns the account or is an admin...");

        var user = userContext.GetCurrentUser();

        if (user is not null)
        {
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}