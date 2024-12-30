namespace ForkPoint.Application.Models.Emails;

public interface IEmailTemplate
{
    public string Subject { get; }

    public string Content { get; }
}