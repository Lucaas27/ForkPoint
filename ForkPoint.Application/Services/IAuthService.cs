using System.Security.Claims;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Services;

public interface IAuthService
{
    public ClaimsPrincipal? GetPrincipalFromToken(string token);
    Task<string> GenerateToken(User user);
    string GenerateRefreshToken();
}