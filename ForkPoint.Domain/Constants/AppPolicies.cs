namespace ForkPoint.Domain.Constants;

public static class AppPolicies
{
    public const string AdminPolicy = "Admin"; // Policy reserved just for Admins.

    public const string
        AdminOrOwnerPolicy = "AdminOrOwner"; // Policy reserved for Owners. Admins can do everything owners can.

    public const string
        OwnsRestaurantOrAdminPolicy =
            "OwnsRestaurantOrAdmin"; // This will prevent users from updating/deleting restaurants they don't own.

    public const string
        OwnsResourceOrAdminPolicy =
            "OwnsResourceOrAdmin"; // This will prevent users from updating records they don't own.
    // A user policy is not needed as this will the default access a user will have which can then be upgrade to either owner or admin.
}