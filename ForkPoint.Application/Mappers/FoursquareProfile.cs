using AutoMapper;
using ForkPoint.Application.ExternalClients.Foursquare;
using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Mappers;

public class FoursquareProfile : Profile
{
    public FoursquareProfile()
    {
        CreateMap<Place, RestaurantModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new Random().Next()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => GetDescription(src)))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => GetCategory(src)))
            .ForMember(dest => dest.Website,
                opt => opt.MapFrom(src => src.Website ?? string.Empty));
    }

    private static string GetDescription(Place? src)
    {
        if (src == null)
        {
            return string.Empty;
        }

        var location = src.Location;

        if (location != null && !string.IsNullOrEmpty(location.FormattedAddress))
        {
            return $"{location.FormattedAddress} - {location.Locality} - {location.Country} ";
        }

        if (location != null && !string.IsNullOrEmpty(location.Address))
        {
            return location.Address;
        }

        return string.Empty;
    }

    private static string GetCategory(Place? src)
    {
        if (src?.Categories == null || src.Categories.Count == 0)
        {
            return "Uncategorized";
        }

        var first = src.Categories[0];

        return string.IsNullOrEmpty(first.Name) ? "Uncategorized" : first.Name;
    }
}
