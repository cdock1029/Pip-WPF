using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Td.Migrations
{
    /// <inheritdoc />
    public partial class InvestmentMaturityDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "AuctionDate",
                table: "Investments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuctionDate",
                value: new DateOnly(2024, 7, 25));

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuctionDate",
                value: new DateOnly(2024, 6, 18));

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AuctionDate",
                value: new DateOnly(2024, 5, 9));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionDate",
                table: "Investments");
        }
    }
}
