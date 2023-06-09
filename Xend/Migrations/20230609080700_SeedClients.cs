using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xend.Migrations
{
    /// <inheritdoc />
    public partial class SeedClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "Email", "Name" },
                values: new object[,]
                {
                    { "1", "john.doe@example.com", "John Doe" },
                    { "10", "jessica.clark@example.com", "Jessica Clark" },
                    { "2", "jane.smith@example.com", "Jane Smith" },
                    { "3", "mike.johnson@example.com", "Mike Johnson" },
                    { "4", "sarah.williams@example.com", "Sarah Williams" },
                    { "5", "robert.brown@example.com", "Robert Brown" },
                    { "6", "emily.davis@example.com", "Emily Davis" },
                    { "7", "david.wilson@example.com", "David Wilson" },
                    { "8", "jennifer.taylor@example.com", "Jennifer Taylor" },
                    { "9", "michael.anderson@example.com", "Michael Anderson" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "9");
        }
    }
}
