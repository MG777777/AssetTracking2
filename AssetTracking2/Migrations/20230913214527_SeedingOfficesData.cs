using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetTracking2.Migrations
{
    public partial class SeedingOfficesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Currency", "Name" },
                values: new object[] { 1, "SEK", "Sweden" });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Currency", "Name" },
                values: new object[] { 2, "EUR", "Spain" });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Currency", "Name" },
                values: new object[] { 3, "USD", "Usa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
