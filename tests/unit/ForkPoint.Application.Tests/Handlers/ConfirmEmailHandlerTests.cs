using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class ConfirmEmailHandlerTests
{
    private readonly Mock<ILogger<ConfirmEmailHandler>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly ConfirmEmailHandler _handler;

    public ConfirmEmailHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ConfirmEmailHandler>>();
        _userManagerMock = MockUserManager<User>();
        _handler = new ConfirmEmailHandler(_loggerMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenEmailIsConfirmed()
    {
        // Arrange
        var request = new ConfirmEmailRequest("valid-token", "test@example.com");
        var user = new User { Email = "test@example.com" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, request.Token)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Email confirmed");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new ConfirmEmailRequest("valid-token", "test@example.com");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Payload is invalid");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailConfirmationFails()
    {
        // Arrange
        var request = new ConfirmEmailRequest("valid-token", "test@example.com");
        var user = new User { Email = "test@example.com" };
        var identityErrors = new IdentityError[] { new() { Description = "Invalid token" } };
        var identityResult = IdentityResult.Failed(identityErrors);

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, request.Token)).ReturnsAsync(identityResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Failed to confirm email. Invalid token");
    }

    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }
}
