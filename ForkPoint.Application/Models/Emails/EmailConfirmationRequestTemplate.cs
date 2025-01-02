namespace ForkPoint.Application.Models.Emails;

public record EmailConfirmationRequestTemplate : IEmailTemplate
{
    private readonly string? _baseUrl;
    private readonly string? _email;
    private readonly string? _token;

    public EmailConfirmationRequestTemplate(string? baseUrl, string token, string email)
    {
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public string Subject => "Confirm your email with ForkPoint";

    public string Content => $"""
                              <div style='text-align: left;'>
                                  <p>Click the link below to confirm your email or enter the code <i>{_token}</i> in the browser:</p>
                                  <a href='{_baseUrl}/api/auth/confirmEmail?email={Uri.EscapeDataString(_email!)}&token={Uri.EscapeDataString(_token!)}'>Confirm email</a>
                              </div>
                              """;
}