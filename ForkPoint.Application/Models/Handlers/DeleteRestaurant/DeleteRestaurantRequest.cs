using MediatR;

namespace ForkPoint.Application.Models.Handlers.DeleteRestaurant;
public record DeleteRestaurantRequest : IRequest<DeleteRestaurantResponse>
{
    public int Id { get; }

    public DeleteRestaurantRequest(int id)
    {
        Id = id;
    }

}
