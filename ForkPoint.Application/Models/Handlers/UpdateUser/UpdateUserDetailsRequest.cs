using MediatR;

namespace ForkPoint.Application.Models.Handlers.UpdateUser;

public record UpdateUserDetailsRequest(string FullName) : IRequest<UpdateUserDetailsResponse>;