using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Persistence;
internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User>(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; } = null!;
    internal DbSet<MenuItem> MenuItems { get; set; } = null!;
    internal DbSet<Address> Addresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This applies all configurations specified in types implementing IEntityTypeConfiguration from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
