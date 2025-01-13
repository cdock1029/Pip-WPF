using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Td.Migrations
{
    /// <inheritdoc />
    public partial class Incremental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Confirmation",
                value: "FOOXX");

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Confirmation",
                value: "BARXX");

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Confirmation",
                value: "BAZXX");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Confirmation",
                value: "FOO");

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Confirmation",
                value: "BAR");

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Confirmation",
                value: "BAZ");
        }
    }
}
