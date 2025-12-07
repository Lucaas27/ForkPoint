using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;

internal class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await userManager.Users.CountAsync(cancellationToken);
    }

    public async Task<List<User>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var page = pageNumber <= 0 ? 1 : pageNumber;
        var size = pageSize <= 0 ? 10 : pageSize;

        var users = await userManager.Users
            .OrderBy(u => u.Id)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<User?> FindByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<User?> FindByLoginAsync(string provider, string providerKey)
    {
        return await userManager.FindByLoginAsync(provider, providerKey);
    }

    public async Task<User?> GetUserWithOwnedRestaurantsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await userManager.Users
            .Include(u => u.OwnedRestaurants)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(User user, string? password = null)
    {
        return password == null ? await userManager.CreateAsync(user) : await userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        return await userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
    {
        return await userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login)
    {
        return await userManager.AddLoginAsync(user, login);
    }

    public async Task<string?> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName)
    {
        return await userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
    }

    public async Task SetAuthenticationTokenAsync(User user, string loginProvider, string tokenName, string value)
    {
        await userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, value);
    }

    public async Task<string> GenerateUserTokenAsync(User user, string provider, string purpose)
    {
        return await userManager.GenerateUserTokenAsync(user, provider, purpose);
    }

    public async Task<bool> VerifyUserTokenAsync(User user, string provider, string purpose, string token)
    {
        return await userManager.VerifyUserTokenAsync(user, provider, purpose, token);
    }

    public async Task RemoveAuthenticationTokenAsync(User user, string provider, string tokenName)
    {
        await userManager.RemoveAuthenticationTokenAsync(user, provider, tokenName);
    }

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        return await userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }
}
