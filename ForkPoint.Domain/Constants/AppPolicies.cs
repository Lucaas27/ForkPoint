namespace ForkPoint.Domain.Constants;

public static class AppPolicies
{
    public const string AdminPolicy = "Admin";
    public const string OwnerPolicy = "Owner";
    public const string UserPolicy = "User";
    public const string AdminOrOwnerPolicy = "AdminOrOwner";
    public const string OwnsRestaurantOrAdminPolicy = "OwnsRestaurantOrAdmin";
    public const string OwnsResourceOrAdminPolicy = "OwnsResourceOrAdmin";
}