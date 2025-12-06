namespace ForkPoint.Application.Models.Emails;

public class PasswordResetTemplate(string callback, string destination) : IEmailTemplate
{
    public string Subject => "Reset your password - ForkPoint";

    public string Html => $"""
                           <div style='text-align: center; background-color: white'>
                               <div style='text-align: left;'>
                                   <h3>Reset your password</h3>
                                   <p>We received a request to reset the password for your account, which uses this email address.
                                      If you made this request, just click the link below to securely reset your password:
                                     </p>
                                   <p>{callback}</p>

                                   <p>For security purposes, this link will expire in 10 minutes.</p>
                               </div>
                           </div>
                           """;

    public string Text => string.Empty;

    public string Destination { get; } =
        destination ?? throw new ArgumentNullException(nameof(destination), "Destination is required.");
}