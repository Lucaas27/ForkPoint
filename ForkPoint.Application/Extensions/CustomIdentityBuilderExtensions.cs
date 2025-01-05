using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ForkPoint.Application.Extensions;

public static class CustomIdentityBuilderExtensions
{
    public static IdentityBuilder AddCustomPasswordTokenProvider(this IdentityBuilder builder)
    {
        var userType = builder.UserType;
        var passwordProvider = typeof(CustomPasswordTokenProvider<>).MakeGenericType(userType);
        return builder.AddTokenProvider("CustomPasswordTokenProvider", passwordProvider);
    }

    public static IdentityBuilder AddCustomRefreshTokenProvider(this IdentityBuilder builder)
    {
        var userType = builder.UserType;
        var refreshTokenProvider = typeof(CustomRefreshTokenProvider<>).MakeGenericType(userType);
        return builder.AddTokenProvider("CustomRefreshTokenProvider", refreshTokenProvider);
    }
}

public class CustomRefreshTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<CustomRefreshTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger
)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class;

public class CustomRefreshTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public CustomRefreshTokenProviderOptions(
    )
    {
        Name = "CustomRefreshTokenProvider";
        TokenLifespan = TimeSpan.FromHours(12);
    }
}

public class CustomPasswordTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<CustomPasswordTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger
)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class;

public class CustomPasswordTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public CustomPasswordTokenProviderOptions()
    {
        Name = "CustomPasswordTokenProvider";
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}