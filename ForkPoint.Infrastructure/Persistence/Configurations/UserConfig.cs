using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForkPoint.Infrastructure.Persistence.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FullName)
            .HasMaxLength(100);

        // Configures the relationship between User and Restaurant entities
        builder.HasMany(u => u.OwnedRestaurants) // A User has many Restaurants
            .WithOne(r => r.Owner) // Each Restaurant has one Owner
            .HasForeignKey(r => r.OwnerId) // The foreign key in Restaurant is OwnerId
            .OnDelete(DeleteBehavior.Cascade); // When a User is deleted, their Restaurants are also deleted
    }
}