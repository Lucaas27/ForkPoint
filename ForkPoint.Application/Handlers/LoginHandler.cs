using ForkPoint.Application.Models.Handlers.LoginUser;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class LoginHandler(ILogger<LoginHandler> logger) : BaseHandler<LoginRequest, LoginResponse>
{
    public override Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}