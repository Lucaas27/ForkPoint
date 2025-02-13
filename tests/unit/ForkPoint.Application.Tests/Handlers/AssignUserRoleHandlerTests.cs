using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.AssignUserRole;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class AssignUserRoleHandlerTests
{
    private readonly Mock<ILogger<AssignUserRoleHandler>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<int>>> _roleManagerMock;
    private readonly AssignUserRoleHandler _handler;

    public AssignUserRoleHandlerTests()
    {
        _loggerMock = new Mock<ILogger<AssignUserRoleHandler>>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(
            Mock.Of<IRoleStore<IdentityRole<int>>>(), null!, null!, null!, null!);
        _handler = new AssignUserRoleHandler(_loggerMock.Object, _userManagerMock.Object, _roleManagerMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new AssignUserRoleRequest("nonexistent@example.com", "Admin");
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_RoleNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var user = new User { Email = "user@example.com" };
        var request = new AssignUserRoleRequest(user.Email, "NonexistentRole");
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(x => x.FindByNameAsync(request.Role)).ReturnsAsync((IdentityRole<int>?)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_AddToRoleFailed_ReturnsFailureResponse()
    {
        // Arrange
        var user = new User { Email = "user@example.com" };
        var role = new IdentityRole<int> { Name = "Admin" };
        var request = new AssignUserRoleRequest(user.Email, role.Name);
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(x => x.FindByNameAsync(request.Role)).ReturnsAsync(role);
        _userManagerMock.Setup(x => x.AddToRoleAsync(user, role.Name)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Contain("Failed to assign role");
    }

    [Fact]
    public async Task Handle_AddToRoleSucceeded_ReturnsSuccessResponse()
    {
        // Arrange
        var user = new User { Email = "user@example.com" };
        var role = new IdentityRole<int> { Name = "Admin" };
        var request = new AssignUserRoleRequest(user.Email, role.Name);
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(x => x.FindByNameAsync(request.Role)).ReturnsAsync(role);
        _userManagerMock.Setup(x => x.AddToRoleAsync(user, role.Name)).ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain("Role Admin assigned to user with email user@example.com");
    }
}
