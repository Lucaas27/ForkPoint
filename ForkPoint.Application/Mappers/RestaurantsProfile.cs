using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Contexts;
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
            .ForMember(d => d.MenuItems, opt => opt.MapFrom(src => src.MenuItems))
            .ForMember(d => d.OwnedByCurrentUser, opt => opt.MapFrom<OwnedByCurrentUserResolver>());

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
            .ForMember(d => d.HasDelivery, opt => opt.MapFrom(src => src.HasDelivery))
            .ForMember(d => d.Email, opt => opt.MapFrom((src, dest) => src.Email ?? dest.Email))
            .ForMember(d => d.ContactNumber, opt => opt.MapFrom((src, dest) => src.ContactNumber ?? dest.ContactNumber))
            .ForMember(d => d.Address, opt => opt.MapFrom((src, dest) =>
            {
                if (src.Address == null) return dest.Address;
                var address = dest.Address ?? new Address();
                address.Street = src.Address.Street ?? address.Street;
                address.City = src.Address.City ?? address.City;
                address.County = src.Address.County ?? address.County;
                address.PostCode = src.Address.PostCode ?? address.PostCode;
                address.Country = src.Address.Country ?? address.Country;
                return address;
            }));
    }
}

public class OwnedByCurrentUserResolver : IValueResolver<Restaurant, RestaurantDetailsModel, bool>
{
    private readonly IUserContext _userContext;

    public OwnedByCurrentUserResolver(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public bool Resolve(Restaurant source, RestaurantDetailsModel destination, bool destMember, ResolutionContext context)
    {
        var current = _userContext.GetCurrentUser();
        return current is not null && current.Id == source.OwnerId;
    }
}