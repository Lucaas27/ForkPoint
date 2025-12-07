using System.Security.Claims;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Services;

public interface IAuthService
{
    /// <summary>
    ///     Extract the ClaimsPrincipal from a JWT access token.
    /// </summary>
    /// <param name="token">The JWT access token.</param>
    public ClaimsPrincipal? GetPrincipalFromToken(string token);

    /// <summary>
    ///     Generate a new access token for the user.
    /// </summary>
    /// <param name="user">The user for whom to generate the access token.</param>
    Task<string> GenerateAccessToken(User user);

    /// <summary>
    ///     Generate a new refresh token for the user, persist it server-side, and set an HttpOnly cookie on the current response.
    ///     Returns the raw refresh token string.
    /// </summary>
    Task<string> GenerateRefreshToken(User user);

    /// <summary>
    ///     Validate a refresh token for a user.
    /// </summary>
    Task<bool> ValidateRefreshToken(User user, string token);

    /// <summary>
    ///     Invalidate the current refresh token for the user and remove any refresh cookie on the response.
    /// </summary>
    Task InvalidateRefreshToken(User user);

    /// <summary>
    ///     Clear any refresh cookie on the current response.
    /// </summary>
    Task ClearRefreshCookie();

    /// <summary>
    ///     Read the refresh token from the current request's cookies (if any).
    ///     Returns null when not present.
    /// </summary>
    string? GetRefreshTokenFromRequest();
}
