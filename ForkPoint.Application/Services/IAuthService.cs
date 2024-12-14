using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Services;

public interface IAuthService
{
    string GenerateToken(User user);
}