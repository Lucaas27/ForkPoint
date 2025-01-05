namespace ForkPoint.Application.Models.Emails;

public record EmailPasswordResetRequestTemplate : IEmailTemplate
{
    private readonly string? _callback;

    public EmailPasswordResetRequestTemplate(string callback)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public string Subject => "ForkPoint password reset";

    public string Content => $"""
                              <div style='text-align: center; background-color: white'>
                                  <div style='text-align: left;'>
                                      <h3>Reset your password</h3>
                                      <p>We received a request to reset the password for your account, which uses this email address. 
                                         If you made this request, just click the link below to securely reset your password:
                                        </p>
                                      <p>{_callback}</p>
                                      
                                      <p>For security purposes, this link will expire in ten minutes.</p>
                                  </div>
                              </div>
                              """;
}