using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForkPoint.Application.Services;
public class TokenService(IConfiguration config, UserManager<User> userManager) : ITokenService
{
    public async Task<string> GenerateToken(User user)
    {
        var clientId = config["Authentication:Google:ClientId"];
        if (clientId == null)
        {
            throw new ArgumentNullException(nameof(clientId), "Authentication:Google:ClientId is missing in appsettings.json");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientId));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Get user claims and roles
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.UserName!)
        };

        claims.AddRange(userClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials,
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}
