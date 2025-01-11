using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Contexts;

public interface IUserContext
{
    CurrentUserModel? GetCurrentUser();
    int GetTargetUserId();
}