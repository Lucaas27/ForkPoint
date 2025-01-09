using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ForkPoint.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b615914e-01be-4a3e-be67-bfb16f4e456b");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bac10e6e-0a31-4415-b572-e4600cf76ffd");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "f635f30c-527a-491b-8bc8-71ac707db9b6");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6203cfad-3e1b-4dee-b83c-c7929ce2e82d");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "66af68cb-b112-4273-aa18-cc8d570bfdf4");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a09e2d9a-b0c4-4816-9be7-fda087ebf1df");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b615914e-01be-4a3e-be67-bfb16f4e456b", null, "Admin", null },
                    { "bac10e6e-0a31-4415-b572-e4600cf76ffd", null, "Owner", null },
                    { "f635f30c-527a-491b-8bc8-71ac707db9b6", null, "User", null }
                });
        }
    }
}
