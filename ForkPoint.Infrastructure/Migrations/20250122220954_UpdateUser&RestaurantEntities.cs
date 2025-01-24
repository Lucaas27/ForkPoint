using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkPoint.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdateUserRestaurantEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Implementation for the Up method
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // This method is intentionally left empty because the migration is not reversible.
        // If you need to reverse this migration, you should implement the necessary logic here.
        throw new NotSupportedException("This migration cannot be reversed.");
    }
}
