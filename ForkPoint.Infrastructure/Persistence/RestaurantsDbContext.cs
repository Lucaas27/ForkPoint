using ForkPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Persistence;
internal class RestaurantsDbContext : DbContext
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<MenuItem> MenuItems { get; set; }
    internal DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ForkPoint;Trusted_Connection=True;TrustServerCertificate=True");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This applies all configurations specified in types implementing IEntityTypeConfiguration from the assembly 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantsDbContext).Assembly);
    }
}
