using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Domain.Entities;

// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

namespace ForkPoint.Application.Mappers;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantModel>();

        CreateMap<Restaurant, RestaurantDetailsModel>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(d => d.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

        CreateMap<CreateRestaurantRequest, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => new Address
            {
                Street = src.Address.Street,
                City = src.Address.City,
                County = src.Address.County,
                PostCode = src.Address.PostCode,
                Country = src.Address.Country
            })
            );

        CreateMap<UpdateRestaurantRequest, Restaurant>()
            .ForMember(d => d.Name, opt => opt.MapFrom((src, dest) => src.Name ?? dest.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom((src, dest) => src.Description ?? dest.Description))
            .ForMember(d => d.HasDelivery, opt => opt.MapFrom((src, dest) => src.HasDelivery ?? dest.HasDelivery))
            .ForMember(d => d.Email, opt => opt.MapFrom((src, dest) => src.Email ?? dest.Email))
            .ForMember(d => d.ContactNumber, opt => opt.MapFrom((src, dest) => src.ContactNumber ?? dest.ContactNumber))
            .ForMember(d => d.Address, opt => opt.MapFrom((src, dest) => src.Address != null
                ? new Address
                {
                    Street = src.Address.Street ?? dest.Address.Street,
                    City = src.Address.City ?? dest.Address.City,
                    County = src.Address.County ?? dest.Address.County,
                    PostCode = src.Address.PostCode ?? dest.Address.PostCode,
                    Country = src.Address.Country ?? dest.Address.Country
                }
                : dest.Address));
    }
}