using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Domain.Entities;

public class User : IdentityUser
{
    public string FullName { get; init; } = null!;
    public string? RefreshToken { get; init; }
    public DateTime RefreshTokenExpiryTime { get; init; } = new(1900, 1, 1);
}
