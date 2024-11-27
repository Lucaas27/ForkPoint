using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
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

        CreateMap<NewRestaurantRequest, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => new Address
            {
                Street = src.Street,
                City = src.City,
                County = src.County,
                PostCode = src.PostCode,
                Country = src.Country
            })
            );
    }
}
