using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;
public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        /// <summary>
        /// Maps the properties of a <see cref="Restaurant"/> entity to a <see cref="RestaurantModel"/>.
        /// </summary>
        /// <remarks>
        /// This mapping includes all the basic properties of the <see cref="Restaurant"/> entity.
        /// </remarks>
        /// <example>
        /// If the <see cref="Restaurant.Name"/> is "The Great Restaurant", the <see cref="RestaurantModel.Name"/> will be "The Great Restaurant".
        /// </example>
        CreateMap<Restaurant, RestaurantModel>();

        /// <summary>
        /// Maps the properties of a <see cref="Restaurant"/> entity to a <see cref="RestaurantDetailsModel"/>.
        /// </summary>
        /// <remarks>
        /// This mapping includes the <see cref="Address"/> and <see cref="MenuItems"/> properties.
        /// </remarks>
        /// <example>
        /// If the <see cref="Restaurant.Address"/> is "123 Main St", the <see cref="RestaurantDetailsModel.Address"/> will be "123 Main St".
        /// </example>
        CreateMap<Restaurant, RestaurantDetailsModel>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(d => d.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

        /// <summary>
        /// Maps the properties of a <see cref="NewRestaurantRequest"/> to a new <see cref="Restaurant"/> entity.
        /// </summary>
        /// <remarks>
        /// This mapping creates a new <see cref="Address"/> object for the <see cref="Restaurant"/> entity using the properties from the <see cref="NewRestaurantRequest"/>.
        /// </remarks>
        /// <example>
        /// If the <see cref="NewRestaurantRequest.Street"/> is "123 Main St", the <see cref="Restaurant.Address.Street"/> will be "123 Main St".
        /// </example>
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

        /// <summary>
        /// Maps the properties of an <see cref="UpdateRestaurantRequest"/> to an existing <see cref="Restaurant"/> entity.
        /// </summary>
        /// <remarks>
        /// This mapping ensures that only non-null properties from the source object overwrite the destination object.
        /// </remarks>
        /// <example>
        /// If the <see cref="UpdateRestaurantRequest.Name"/> is null, the existing <see cref="Restaurant.Name"/> will remain unchanged.
        /// </example>
        CreateMap<UpdateRestaurantRequest, Restaurant>()
            .ForMember(d => d.Name, opt => opt.MapFrom((src, dest) => src.Name ?? dest.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom((src, dest) => src.Description ?? dest.Description))
            .ForMember(d => d.HasDelivery, opt => opt.MapFrom((src, dest) => src.HasDelivery ?? dest.HasDelivery))
            .ForMember(d => d.Email, opt => opt.MapFrom((src, dest) => src.Email ?? dest.Email))
            .ForMember(d => d.ContactNumber, opt => opt.MapFrom((src, dest) => src.ContactNumber ?? dest.ContactNumber))
            .ForMember(d => d.Address, opt => opt.MapFrom((src, dest) => src.Address != null ? new Address
            {
                Street = src.Address.Street ?? dest.Address.Street,
                City = src.Address.City ?? dest.Address.City,
                County = src.Address.County ?? dest.Address.County,
                PostCode = src.Address.PostCode ?? dest.Address.PostCode,
                Country = src.Address.Country ?? dest.Address.Country
            } : dest.Address));
    }
}
