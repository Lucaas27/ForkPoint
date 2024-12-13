using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Services;

public interface ITokenService
{
    Task<string> GenerateToken(User user);
}