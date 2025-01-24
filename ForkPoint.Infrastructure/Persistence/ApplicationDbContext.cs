using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Persistence;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; } = null!;
    internal DbSet<MenuItem> MenuItems { get; set; } = null!;
    internal DbSet<Address> Addresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // This applies all configurations specified in types implementing IEntityTypeConfiguration from the assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}