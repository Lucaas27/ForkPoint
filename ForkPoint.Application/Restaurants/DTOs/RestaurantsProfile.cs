using AutoMapper;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Restaurants.DTOs;
public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDTO>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(d => d.MenuItems, opt => opt.MapFrom(src => src.MenuItems));
    }
}
