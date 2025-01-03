namespace ForkPoint.Application.Models.Emails;

public record EmailTemplateParameters
{
    public string? Token { get; init; }
    public string? UserEmail { get; init; }
    public string? Callback { get; init; }
}