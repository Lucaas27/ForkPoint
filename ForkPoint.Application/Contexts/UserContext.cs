using System.Security.Claims;
using ForkPoint.Application.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace ForkPoint.Application.Contexts;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUserModel? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            throw new InvalidOperationException("User not found in the current context");
        }

        if (user.Identity is not { IsAuthenticated: true })
        {
            return null;
        }

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var roles = user.FindAll(ClaimTypes.Role).Select(claim => claim.Value);

        return new CurrentUserModel(int.Parse(id), email, roles);
    }

    public int GetTargetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.Request.RouteValues["userId"]?.ToString();

        if (userId is null)
        {
            throw new InvalidOperationException("User ID not found in the current context");
        }

        if (!int.TryParse(userId, out var id))
        {
            throw new InvalidCastException("User ID is not an INT");
        }

        return id;
    }
}