using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkPoint.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KiloCaloriesAddedToMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KiloCalories",
                table: "MenuItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KiloCalories",
                table: "MenuItems");
        }
    }
}
