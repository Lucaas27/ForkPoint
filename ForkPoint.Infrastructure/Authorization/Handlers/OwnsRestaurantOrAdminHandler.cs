using ForkPoint.Application.Contexts;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Infrastructure.Authorization.Handlers;

public class OwnsRestaurantOrAdminHandler(
    ILogger<OwnsRestaurantOrAdminHandler> logger,
    IRestaurantRepository restaurantRepository,
    IUserContext userContext,
    IRestaurantContext restaurantContext
)
    : AuthorizationHandler<OwnsRestaurantOrAdminRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnsRestaurantOrAdminRequirement ownsRestaurantOrAdminOrAdminRequirement
    )
    {
        logger.LogInformation("Checking if user owns the restaurant or is an admin...");

        var user = userContext.GetCurrentUser();
        var restaurantId = restaurantContext.GetTargetedRestaurantId();

        if (user is null)
        {
            context.Fail();
            return;
        }

        if (user.IsInRole("Admin"))
        {
            context.Succeed(ownsRestaurantOrAdminOrAdminRequirement);
            return;
        }

        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(restaurantId);

        if (restaurant is not null && restaurant.OwnerId == user.Id)
        {
            context.Succeed(ownsRestaurantOrAdminOrAdminRequirement);
        } else
        {
            context.Fail();
        }
    }
}