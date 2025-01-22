using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Contexts;

public interface IUserContext
{
    bool IsInRole(string role);
    CurrentUserModel? GetCurrentUser();
    int GetTargetUserId();
}