using FluentAssertions;
using ForkPoint.Application.Factories;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Emails;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Tests.Handlers;

public class ForgotPasswordHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenUserExists()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ForgotPasswordHandler>>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var emailServiceMock = new Mock<IEmailService>();
        var emailTemplateFactoryMock = new Mock<IEmailTemplateFactory>();

        var user = new User { Email = "test@example.com" };
        var request = new ForgotPasswordRequest("test@example.com");
        var token = "reset-token";
        var emailTemplate = new PasswordResetTemplate("callback", "test@example.com");

        userRepositoryMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        userRepositoryMock.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(token);
        emailTemplateFactoryMock.Setup(etf => etf.CreatePasswordResetTemplate(It.IsAny<string>(), It.IsAny<string>())).Returns(emailTemplate);
        emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<IEmailTemplate>())).Returns(Task.CompletedTask);

        var handler = new ForgotPasswordHandler(loggerMock.Object, userRepositoryMock.Object, emailServiceMock.Object, emailTemplateFactoryMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Please check your email and follow the instructions on how to reset your password.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenUserDoesNotExist()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ForgotPasswordHandler>>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var emailServiceMock = new Mock<IEmailService>();
        var emailTemplateFactoryMock = new Mock<IEmailTemplateFactory>();

        var request = new ForgotPasswordRequest("test@example.com");

        userRepositoryMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var handler = new ForgotPasswordHandler(loggerMock.Object, userRepositoryMock.Object, emailServiceMock.Object, emailTemplateFactoryMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Please check your email and follow the instructions on how to reset your password.");
    }
}
