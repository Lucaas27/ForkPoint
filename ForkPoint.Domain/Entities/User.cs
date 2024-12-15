﻿using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; init; } = null!;

    public override string? UserName
    {
        get => base.UserName ?? Email;
        set => base.UserName = value ?? Email;
    }

    public string? RefreshToken { get; init; }
    public DateTime RefreshTokenExpiryTime { get; init; } = new(1900, 1, 1);
}