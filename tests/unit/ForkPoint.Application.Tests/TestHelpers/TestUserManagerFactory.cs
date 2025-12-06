using Microsoft.AspNetCore.Identity;
using Moq;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Tests.TestHelpers;

public static class TestUserManagerFactory
{
    public static Mock<UserManager<User>> CreateMinimal()
    {
        var mock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        return mock;
    }

    public static Mock<UserManager<User>> CreateWithUser(User? user)
    {
        var mock = CreateMinimal();
        mock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => user != null && email == user.Email ? user : null);
        mock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        mock.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        mock.Setup(u => u.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("token");
        return mock;
    }
}

