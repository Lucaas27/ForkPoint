using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForkPoint.Infrastructure.Persistence.Configurations;
internal class MenuItemConfig : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        // Configure the MenuItem entity
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Description).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(m => m.ImageUrl).HasMaxLength(50).IsRequired(false);
        builder.Property(m => m.IsVegetarian).HasDefaultValue(false).IsRequired();
        builder.Property(m => m.IsVegan).HasDefaultValue(false).IsRequired();
        builder.Property(m => m.RestaurantId).IsRequired();
    }
}
