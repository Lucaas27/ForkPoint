using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ForkPoint.Application.Tests.TestHelpers;

namespace ForkPoint.Application.Tests.Handlers;

public class LoginHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValidAndEmailConfirmed()
    {
        // Arrange
        var user = new User { UserName = "user", Email = "user@example.com", EmailConfirmed = true };
        var userManagerMock = TestUserManagerFactory.CreateWithUser(user);
        var signInManagerMock = MockSignInManager(userManagerMock.Object);
        var authServiceMock = new Mock<IAuthService>();

        var dummyToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c2VyIn0.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        authServiceMock.Setup(a => a.GenerateAccessToken(It.IsAny<User>())).ReturnsAsync(dummyToken);
        authServiceMock.Setup(a => a.GenerateRefreshToken(It.IsAny<User>())).ReturnsAsync("refresh");

        var handler = new LoginHandler(new Mock<ILogger<LoginHandler>>().Object, authServiceMock.Object, userManagerMock.Object, signInManagerMock.Object);

        var request = new LoginRequest("user@example.com", "Password123!");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.AccessToken.Should().Be(dummyToken);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEmailNotConfirmed()
    {
        // Arrange
        var user = new User { UserName = "user", Email = "user@example.com", EmailConfirmed = false };
        var userManagerMock = TestUserManagerFactory.CreateWithUser(user);
        var signInManagerMock = MockSignInManager(userManagerMock.Object);
        var authServiceMock = new Mock<IAuthService>();

        var handler = new LoginHandler(new Mock<ILogger<LoginHandler>>().Object, authServiceMock.Object, userManagerMock.Object, signInManagerMock.Object);

        var request = new LoginRequest("user@example.com", "Password123!");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Contain("Please confirm your email");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserMissing()
    {
        // Arrange
        var userManagerMock = TestUserManagerFactory.CreateWithUser(null);
        var signInManagerMock = MockSignInManager(userManagerMock.Object);
        var authServiceMock = new Mock<IAuthService>();

        var handler = new LoginHandler(new Mock<ILogger<LoginHandler>>().Object, authServiceMock.Object, userManagerMock.Object, signInManagerMock.Object);

        var request = new LoginRequest("missing@example.com", "Password123!");

        // Act
        var act = async () => await handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        var mockSignInManager = new Mock<SignInManager<User>>(
            userManager,
            contextAccessor.Object,
            claimsFactory.Object,
            null!,
            null!,
            null!,
            null!
        );

        mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        return mockSignInManager;
    }
}
