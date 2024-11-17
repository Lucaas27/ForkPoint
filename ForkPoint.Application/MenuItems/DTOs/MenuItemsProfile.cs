using AutoMapper;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.MenuItems.DTOs;
public class MenuItemsProfile : Profile
{
    public MenuItemsProfile()
    {
        CreateMap<MenuItem, MenuItemDTO>();
    }
}
