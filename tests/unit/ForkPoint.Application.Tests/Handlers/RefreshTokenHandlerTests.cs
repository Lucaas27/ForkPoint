using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.RefreshToken;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace ForkPoint.Application.Tests.Handlers;

public class RefreshTokenHandlerTests
{
    private readonly Mock<ILogger<LoginHandler>> _loggerMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        _loggerMock = new Mock<ILogger<LoginHandler>>();
        _authServiceMock = new Mock<IAuthService>();
        _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _handler = new RefreshTokenHandler(_loggerMock.Object, _authServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidAccessToken_WhenPrincipalIsNull()
    {
        // Arrange
        var request = new RefreshTokenRequest("invalidToken", "refreshToken");
        _authServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>())).Returns((ClaimsPrincipal)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid access token");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUser_WhenUserIsNull()
    {
        // Arrange
        var request = new RefreshTokenRequest("invalidToken", "refreshToken");
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.Email, "test@example.com") }));
        _authServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>())).Returns(principal);
        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid user");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidRefreshToken_WhenTokenIsInvalid()
    {
        // Arrange
        var request = new RefreshTokenRequest("validToken", "invalidRefreshToken");
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.Email, "test@example.com") }));
        var user = new User { Email = "test@example.com" };
        _authServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>())).Returns(principal);
        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken")).ReturnsAsync("storedRefreshToken");
        _authServiceMock.Setup(x => x.ValidateRefreshToken(user, It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid refresh token");
    }

    [Fact]
    public async Task Handle_ShouldReturnNewTokens_WhenRequestIsValid()
    {
        // Arrange
        var request = new RefreshTokenRequest("validToken", "validRefreshToken");
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.Email, "test@example.com") }));
        var user = new User { Email = "test@example.com" };
        var newAccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9."
                        + "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6"
                        + "IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ."
                        + "SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        var newRefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9."
                        + "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6"
                        + "IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ."
                        + "SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw7c";

        _authServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>())).Returns(principal);
        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken")).ReturnsAsync("storedRefreshToken");
        _authServiceMock.Setup(x => x.ValidateRefreshToken(user, It.IsAny<string>())).ReturnsAsync(true);
        _authServiceMock.Setup(x => x.GenerateAccessToken(user)).ReturnsAsync(newAccessToken);
        _authServiceMock.Setup(x => x.GenerateRefreshToken(user)).ReturnsAsync(newRefreshToken);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Token refreshed");
        result.Token.Should().Be(newAccessToken);
        result.RefreshToken.Should().Be(newRefreshToken);
    }
}
