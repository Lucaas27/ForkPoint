namespace ForkPoint.Application.Models.Emails;

public class EmailConfirmationTemplate(string callback, string destination, string token) : IEmailTemplate
{
    private readonly string _token = token ?? throw new ArgumentNullException(nameof(token), "Token is required.");

    public string Destination { get; } =
        destination ?? throw new ArgumentNullException(nameof(destination), "Destination is required.");

    public string Subject => "Confirm your email with ForkPoint";

    public string Html => $"""
                           <div style='text-align: left;'>
                               <p>Click the link below to confirm your email or enter the code <i>{_token}</i> in the browser:</p>
                               <a href='{callback}'>Confirm email</a>
                           </div>
                           """;

    public string Text => string.Empty;
}