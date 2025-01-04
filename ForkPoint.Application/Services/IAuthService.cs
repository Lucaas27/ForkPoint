using System.Security.Claims;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Services;

public interface IAuthService
{
    public ClaimsPrincipal? GetPrincipalFromToken(string token);
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    Task<bool> ValidateRefreshToken(User user, string token);
}