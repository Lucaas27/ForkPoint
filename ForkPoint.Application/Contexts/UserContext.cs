using System.Security.Claims;
using ForkPoint.Application.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace ForkPoint.Application.Contexts;

public record UserContext(IHttpContextAccessor HttpContextAccessor) : IUserContext
{
    public CurrentUserModel? GetCurrentUser()
    {
        var user = HttpContextAccessor.HttpContext?.User ??
                   throw new InvalidOperationException("User not found in the current context");

        if (user.Identity is not { IsAuthenticated: true })
        {
            return null;
        }

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var roles = user.FindAll(ClaimTypes.Role).Select(claim => claim.Value);
        var name = user.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        return new CurrentUserModel(int.Parse(id), email, roles, name);
    }

    public int GetTargetUserId()
    {
        var userId = HttpContextAccessor.HttpContext?.Request.RouteValues["userId"]?.ToString()
                     ?? throw new InvalidOperationException("User ID not found in the current context");

        if (!int.TryParse(userId, out var id))
        {
            throw new InvalidCastException("User ID is not an INT");
        }

        return id;
    }

    public bool IsInRole(string role)
    {
        var user = GetCurrentUser() ?? throw new InvalidOperationException("User not found in the current context");

        return user.Roles.Contains(role);
    }
}
