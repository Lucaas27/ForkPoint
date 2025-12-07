using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.GetUsers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Tests.Handlers;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsPaginatedUsers()
    {
        // Arrange
        var user1 = new User { Id = 1, Email = "user1@example.com", FullName = "User One", UserName = "user1" };

        var user2 = new User { Id = 2, Email = "user2@example.com", FullName = "User Two", UserName = "user2" };

        var users = new List<User> { user1, user2 };

        var userRepoMock = new Mock<IUserRepository>();

        userRepoMock.Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users.Count);

        userRepoMock.Setup(r => r.GetUsersPageAsync(1, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { user1 });

        userRepoMock.Setup(r => r.GetRolesAsync(It.Is<User>(x => x.Email == user1.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string> { "Admin" });

        userRepoMock.Setup(r => r.GetRolesAsync(It.Is<User>(x => x.Email == user2.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string> { "User" });

        var handler = new GetUsersHandler(userRepoMock.Object);

        var request = new GetUsersRequest(1, 1);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.TotalItemsCount.Should().Be(2);
        response.Items.Should().HaveCount(1);
        var first = response.Items.First();
        first.Id.Should().Be(1);
        first.Email.Should().Be("user1@example.com");
        first.Roles.Should().Contain("Admin");
    }
}
