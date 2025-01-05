using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Domain.Entities;

public class User : IdentityUser<int>
{
    public string? FullName { get; init; }

    public override string? UserName
    {
        get => base.UserName ?? Email;
        set => base.UserName = value ?? Email;
    }
}