namespace ForkPoint.Application.Models.Emails;

public record EmailConfirmationRequestTemplate : IEmailTemplate
{
    private readonly string? _callback;
    private readonly string? _token;

    public EmailConfirmationRequestTemplate(string? callback, string token)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _token = token ?? throw new ArgumentNullException(nameof(token));
    }

    public string Subject => "Confirm your email with ForkPoint";

    public string Content => $"""
                              <div style='text-align: left;'>
                                  <p>Click the link below to confirm your email or enter the code <i>{_token}</i> in the browser:</p>
                                  <a href='{_callback}'>Confirm email</a>
                              </div>
                              """;
}