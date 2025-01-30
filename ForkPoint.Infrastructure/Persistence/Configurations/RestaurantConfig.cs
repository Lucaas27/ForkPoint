using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForkPoint.Infrastructure.Persistence.Configurations;

internal class RestaurantsConfig : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        // Max length for Name is 50
        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        // Max length for Description is 500
        builder.Property(r => r.Description)
            .HasMaxLength(500)
            .IsRequired();

        // Max length for Category is 50
        builder.Property(r => r.Category)
            .HasMaxLength(50)
            .IsRequired();

        // Max length for Email is 50
        builder.Property(r => r.Email)
            .HasMaxLength(50)
            .IsRequired();

        // Max length for ContactNumber is 20
        builder.Property(r => r.ContactNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        // HasDelivery is required
        builder.Property(r => r.HasDelivery)
            .IsRequired()
            .HasDefaultValue(false);

        // Configures the relationship between Restaurant and MenuItem entities
        builder.HasMany(r => r.MenuItems) // A Restaurant has many MenuItems
            .WithOne(m => m.Restaurant) // Each MenuItem has one Restaurant
            .HasForeignKey(m => m.RestaurantId) // The foreign key in MenuItem is RestaurantId
            .OnDelete(DeleteBehavior.Cascade); // When a Restaurant is deleted, its MenuItems are also deleted


        // Configures the relationship between Restaurant and Address entities
        builder.HasOne(r => r.Address) // A Restaurant has one Address
            .WithOne(a => a.Restaurant) // Each Address belongs to one Restaurant
            .HasForeignKey<Address>(a => a.RestaurantId) // The foreign key in Address is RestaurantId
            .OnDelete(DeleteBehavior.Cascade); // When a Restaurant is deleted, its Address is also deleted
    }
}