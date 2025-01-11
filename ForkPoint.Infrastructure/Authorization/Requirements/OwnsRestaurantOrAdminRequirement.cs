using Microsoft.AspNetCore.Authorization;

namespace ForkPoint.Infrastructure.Authorization.Requirements;

public class OwnsRestaurantOrAdminRequirement : IAuthorizationRequirement
{
}