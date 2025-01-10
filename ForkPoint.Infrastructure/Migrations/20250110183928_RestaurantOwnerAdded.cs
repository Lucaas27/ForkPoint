using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ForkPoint.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantOwnerAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql("UPDATE Restaurants SET OwnerId = (SELECT Id FROM AspNetUsers WHERE UserName = 'forkpointadmin@gmail.com')");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6203cfad-3e1b-4dee-b83c-c7929ce2e82d", "487e9a16-1f6a-456d-b16d-f3898f89475d", "User", "USER" },
                    { "66af68cb-b112-4273-aa18-cc8d570bfdf4", "2da1eacb-4ad4-48ab-bf35-340f8c3cf3ee", "Admin", "ADMIN" },
                    { "a09e2d9a-b0c4-4816-9be7-fda087ebf1df", "e7df053f-192e-4549-b188-81a61f05d5f9", "Owner", "OWNER" }
                });
        }
    }
}
