using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Domain.Repositories;

public interface IUserRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(string id);
    Task<User?> FindByLoginAsync(string provider, string providerKey);

    Task<User?> GetUserWithOwnedRestaurantsAsync(string email, CancellationToken cancellationToken = default);

    Task<IdentityResult> CreateAsync(User user, string? password = null);
    Task<IdentityResult> UpdateAsync(User user);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
    Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
    Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login);

    Task<string?> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName);
    Task SetAuthenticationTokenAsync(User user, string loginProvider, string tokenName, string value);
    Task<string> GenerateUserTokenAsync(User user, string provider, string purpose);
    Task<bool> VerifyUserTokenAsync(User user, string provider, string purpose, string token);
    Task RemoveAuthenticationTokenAsync(User user, string provider, string tokenName);

    Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
}
