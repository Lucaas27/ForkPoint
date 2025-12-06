using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class AdminUpdateUserDetailsHandlerTests
{
    private readonly Mock<ILogger<AdminUpdateUserDetailsHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly AdminUpdateUserDetailsHandler _handler;

    public AdminUpdateUserDetailsHandlerTests()
    {
        _loggerMock = new Mock<ILogger<AdminUpdateUserDetailsHandler>>();
        _mapperMock = new Mock<IMapper>();
        _userContextMock = new Mock<IUserContext>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _handler = new AdminUpdateUserDetailsHandler(
            _loggerMock.Object, _mapperMock.Object, _userContextMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotAuthenticated_ThrowsInvalidOperationException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUserModel?)null);

        var request = new AdminUpdateUserDetailsRequest("New FullName");

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_TargetUserIdInvalid_ReturnsFailureResponse()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUserModel(1, "test@example.com", new List<string>(), "Test User"));
        _userContextMock.Setup(x => x.GetTargetUserId()).Returns(0);

        var request = new AdminUpdateUserDetailsRequest("New FullName");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Be("Target User ID is not valid. Value must be greater than 0.");
    }

    [Fact]
    public async Task Handle_TargetUserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUserModel(1, "test@example.com", new List<string>(), "Test User"));
        _userContextMock.Setup(x => x.GetTargetUserId()).Returns(1);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var request = new AdminUpdateUserDetailsRequest("New FullName");

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User();
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUserModel(1, "test@example.com", new List<string>(), "Test User"));
        _userContextMock.Setup(x => x.GetTargetUserId()).Returns(1);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

        var request = new AdminUpdateUserDetailsRequest("New FullName");

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.WithMessage("Failed to update user details: Update failed");
    }

    [Fact]
    public async Task Handle_SuccessfulUpdate_ReturnsSuccessResponse()
    {
        // Arrange
        var user = new User();
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUserModel(1, "test@example.com", new List<string>(), "Test User"));
        _userContextMock.Setup(x => x.GetTargetUserId()).Returns(1);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var request = new AdminUpdateUserDetailsRequest("New FullName");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("User details updated successfully");
    }
}
