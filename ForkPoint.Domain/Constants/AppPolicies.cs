namespace ForkPoint.Domain.Constants;

public static class AppPolicies
{
    public const string AdminPolicy = "Admin"; // Policy reserved just for Admins.

    public const string
        AdminOrOwnerPolicy = "AdminOrOwner"; // Policy reserved for Owners. Admins can do everything owners can.

    public const string
        OwnsRestaurantOrAdminPolicy =
            "OwnsRestaurantOrAdmin"; // This will prevent users from updating/deleting restaurants they don't own.

    // A user policy is not needed.
    // This will be the default access a user will have after registering.
    // The account can then be upgraded to be part of other policies.
}