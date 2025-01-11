using MediatR;

namespace ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;

public record AdminUpdateUserDetailsRequest(string FullName) : IRequest<AdminUpdateUserDetailsResponse>;