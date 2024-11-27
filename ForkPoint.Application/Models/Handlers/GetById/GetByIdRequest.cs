using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetById;
public record GetByIdRequest : IRequest<GetByIdResponse>
{
    public int Id { get; init; }
    public GetByIdRequest(int id) => Id = id;

}
