using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GiviCommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRecordsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "Tbilisi", "TBC Bank", "+995-577-35-53-21", "0160", "Tbilisi", "Bruh street 1" },
                    { 2, "Tbilisi", "Bank of Georgia", "+995-577-36-15-21", "0160", "Tbilisi", "Bruh street 2" },
                    { 3, "Tbilisi", "TBC Bank", "+995-577-35-53-51", "0160", "Tbilisi", "Bruh street 4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
