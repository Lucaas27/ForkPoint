using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; init; }

    public override string? UserName
    {
        get => base.UserName ?? Email;
        set => base.UserName = value ?? Email;
    }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; } = new(1900, 1, 1);
}