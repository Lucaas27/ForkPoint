using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;
public class MenuItemsProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemsProfile"/> class.
    /// Defines the mappings between MenuItem, MenuItemModel, and CreateMenuItemRequest.
    /// </summary>
    public MenuItemsProfile()
    {
        CreateMap<MenuItem, MenuItemModel>();
        CreateMap<CreateMenuItemRequest, MenuItem>();
    }
}
