namespace ForkPoint.Application.Models.Emails;

public interface IEmailTemplate
{
    public string Subject { get; }

    public string Html { get; }

    public string Text { get; }

    public string Destination { get; }
}