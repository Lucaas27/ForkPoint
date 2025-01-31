using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Mappers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Tests.Mappers;

public class AddressesProfileTests
{
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;

    public AddressesProfileTests()
    {
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<AddressesProfile>());
        _mapper = _mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromAddressToAddressModel()
    {
        // Arrange
        var address = new Address
        {
            Id = 1,
            Street = "123 Main St",
            City = "Anytown",
            County = "Anycounty",
            PostCode = "12345",
            Country = "GB"
        };

        // Act
        var addressModel = _mapper.Map<AddressModel>(address);

        // Assert
        addressModel.Should().NotBeNull();
        addressModel.Id.Should().Be(address.Id);
        addressModel.Street.Should().Be(address.Street);
        addressModel.City.Should().Be(address.City);
        addressModel.County.Should().Be(address.County);
        addressModel.PostCode.Should().Be(address.PostCode);
        addressModel.Country.Should().Be(address.Country);
    }
}
