using AutoMapper;
using ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserDetailsRequest, User>();
        CreateMap<AdminUpdateUserDetailsRequest, User>();
    }
}