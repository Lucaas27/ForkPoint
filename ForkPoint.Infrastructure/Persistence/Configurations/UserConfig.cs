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

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(2000);

        builder.Property(u => u.RefreshTokenExpiryTime)
            .HasColumnType("datetime");

        builder.HasIndex(u => u.RefreshToken)
            .IsUnique();
    }
}