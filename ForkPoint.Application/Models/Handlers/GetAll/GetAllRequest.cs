using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAll;
public record GetAllRequest : IRequest<GetAllResponse>
{
}
