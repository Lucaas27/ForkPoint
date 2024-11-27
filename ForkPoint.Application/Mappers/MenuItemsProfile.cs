using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;
public class MenuItemsProfile : Profile
{
    public MenuItemsProfile()
    {
        CreateMap<MenuItem, MenuItemModel>();
    }
}
