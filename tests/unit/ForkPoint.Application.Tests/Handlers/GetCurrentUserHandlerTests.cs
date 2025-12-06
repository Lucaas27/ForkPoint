using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetCurrentUser;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class GetCurrentUserHandlerTests
{
    private readonly Mock<ILogger<GetCurrentUserHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly GetCurrentUserHandler _handler;

    public GetCurrentUserHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetCurrentUserHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new GetCurrentUserHandler(_loggerMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserIsPresent()
    {
        // Arrange
        var expected = new CurrentUserModel(42, "test@example.com", ["User"], "Test User");
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(expected);

        // Act
        var result = await _handler.Handle(new GetCurrentUserRequest(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.User.Should().NotBeNull();
        result.User!.Id.Should().Be(expected.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotAuthenticated_WhenNoUser()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUserModel?)null);

        // Act
        var result = await _handler.Handle(new GetCurrentUserRequest(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Not authenticated");
    }
}
