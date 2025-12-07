namespace ForkPoint.Application.Models.Handlers.GetUsers;

using MediatR;

public record GetUsersRequest(int PageNumber = 1, int PageSize = 10) : IRequest<GetUsersResponse>;
