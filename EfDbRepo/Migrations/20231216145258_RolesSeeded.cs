using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.EfDbRepo.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27000f55-dd3f-42b4-a2f2-a52a68ee019a", "3", "HR", "HR" },
                    { "a06a7344-9877-4e09-b905-b959c47157e4", "1", "Admin", "Admin" },
                    { "f33c43c1-73ce-460d-a507-10b018e757ca", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27000f55-dd3f-42b4-a2f2-a52a68ee019a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a06a7344-9877-4e09-b905-b959c47157e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f33c43c1-73ce-460d-a507-10b018e757ca");
        }
    }
}
