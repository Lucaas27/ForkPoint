using AutoMapper;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Addresses.DTOs;
public class AddressesProfile : Profile
{
    public AddressesProfile()
    {
        CreateMap<Address, AddressDTO>();
    }
}
