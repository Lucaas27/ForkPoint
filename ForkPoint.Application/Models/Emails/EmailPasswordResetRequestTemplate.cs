namespace ForkPoint.Application.Models.Emails;

public record EmailPasswordResetRequestTemplate : IEmailTemplate
{
    private readonly string? _callback;
    private readonly string? _token;

    public EmailPasswordResetRequestTemplate(string token, string callback)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public string Subject => "Here's your code";

    public string Content => $"""
                              <div style='text-align: center; background-color: white'>
                                  <div style='text-align: left;'>
                                      <h3>Reset your password</h3>
                                      <p>We’ve received a request to reset your password for your ForkPoint account. 
                                      If you made this request, please use this code to choose a new password: </p>
                                      
                                      <h3>{_token}</h3>
                                      
                                      <p>For security purposes, this unique code will expire in ten minutes.</p>
                                      
                                      <a href='{_callback}'>Reset Password</a>
                                  </div>
                              </div>
                              """;
}