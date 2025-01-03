using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ForkPoint.Application.Extensions;

public class CustomPasswordResetTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<DataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger
)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class
{
    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var random = new Random();
        var token = random.Next(100000, 999999).ToString(); // Generate a 6-digit numeric token
        return await Task.FromResult(token);
    }
}