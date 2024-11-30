using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetById;
public record GetByIdRequest(int Id) : IRequest<GetByIdResponse>;