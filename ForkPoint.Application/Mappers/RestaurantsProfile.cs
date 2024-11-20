using AutoMapper;
using ForkPoint.Application.Models.Restaurant;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;
public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantModel>();

        CreateMap<Restaurant, RestaurantDetailsModel>()
        .ForMember(d => d.Address, opt => opt.MapFrom(src => src.Address))
        .ForMember(d => d.MenuItems, opt => opt.MapFrom(src => src.MenuItems));
    }
}
