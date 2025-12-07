using FluentAssertions;
using ForkPoint.Application.Factories;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ForkPoint.Application.Models.Emails;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Tests.Handlers;

public class RegisterHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRegisterUser_AndSendConfirmationEmail_WhenAllSucceed()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var emailServiceMock = new Mock<IEmailService>();
        var templateFactoryMock = new Mock<IEmailTemplateFactory>();

        var dummyToken = "email_token";
        userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        userRepositoryMock.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), "User")).ReturnsAsync(IdentityResult.Success);
        userRepositoryMock.Setup(u => u.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(dummyToken);

        templateFactoryMock.Setup(f => f.CreateEmailConfirmationTemplate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new EmailConfirmationTemplate("https://client.example/confirm?token=abc", "to@example.com", dummyToken));

        var handler = new RegisterHandler(
            new Mock<ILogger<RegisterHandler>>().Object,
            userRepositoryMock.Object,
            emailServiceMock.Object,
            templateFactoryMock.Object
        );

        var request = new RegisterRequest("test@example.com", "Password123!");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<IEmailTemplate>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserCreationFails()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var emailServiceMock = new Mock<IEmailService>();
        var templateFactoryMock = new Mock<IEmailTemplateFactory>();

        userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

        var handler = new RegisterHandler(
            new Mock<ILogger<RegisterHandler>>().Object,
            userRepositoryMock.Object,
            emailServiceMock.Object,
            templateFactoryMock.Object
        );

        var request = new RegisterRequest("fail@example.com", "Password123!");

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Contain("User creation failed");
    }
}
