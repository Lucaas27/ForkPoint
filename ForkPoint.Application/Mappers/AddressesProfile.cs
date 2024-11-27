using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Mappers;
public class AddressesProfile : Profile
{
    public AddressesProfile()
    {
        CreateMap<Address, AddressModel>();
    }
}
