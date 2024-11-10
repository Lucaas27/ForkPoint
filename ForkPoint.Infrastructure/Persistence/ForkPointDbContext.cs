using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Persistence;
internal class ForkPointDbContext(DbContextOptions<ForkPointDbContext> options) : DbContext(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<MenuItem> MenuItems { get; set; }
    internal DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This applies all configurations specified in types implementing IEntityTypeConfiguration from the assembly 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForkPointDbContext).Assembly);
    }
}
