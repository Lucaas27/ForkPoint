using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForkPoint.Infrastructure.Persistence.Configurations;
internal class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // Configure the Address entity
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Street).HasMaxLength(100).IsRequired();
        builder.Property(a => a.City).HasMaxLength(50).IsRequired();
        builder.Property(a => a.County).HasMaxLength(50).IsRequired(false);
        builder.Property(a => a.PostCode).HasMaxLength(10).IsRequired();
        builder.Property(a => a.Country).HasMaxLength(50).IsRequired(false);
        builder.Property(a => a.RestaurantId).IsRequired();
    }

}
