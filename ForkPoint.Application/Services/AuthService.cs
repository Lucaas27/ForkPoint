using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using ForkPoint.Domain.Repositories;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

[assembly: InternalsVisibleTo("ForkPoint.Application.Tests")]
namespace ForkPoint.Application.Services;

internal class AuthService(
    IConfiguration config,
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor
) : IAuthService
{
    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Key is null");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = securityKey
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (principal == null)
            {
                return null;
            }

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Key is null");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userRoles = await userRepository.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("email_verified", user.EmailConfirmed.ToString()),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.FullName)
                ? (user.UserName ?? user.Email ?? string.Empty)
                : user.FullName)
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(config.GetValue<int>("Jwt:AccessTokenExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = config.GetValue<string>("Jwt:Issuer"),
            Audience = config.GetValue<string>("Jwt:Audience")
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

        return token;
    }

    public async Task<string> GenerateRefreshToken(User user)
    {
        await InvalidateRefreshToken(user);
        var refreshToken = await userRepository.GenerateUserTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken");
        await userRepository.SetAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken", refreshToken);

        // Set HttpOnly cookie on current response.
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(30)
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshToken(User user, string token)
    {
        return await userRepository.VerifyUserTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken", token);
    }

    public async Task InvalidateRefreshToken(User user)
    {
        await userRepository.RemoveAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken");

        httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

    }

    public async Task ClearRefreshCookie()
    {
        // Remove the refresh cookie
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
        await Task.CompletedTask;
    }

    public string? GetRefreshTokenFromRequest()
    {
        return httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("refreshToken", out var t) == true ? t : null;
    }
}
