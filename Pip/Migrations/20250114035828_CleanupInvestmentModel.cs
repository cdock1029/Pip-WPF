using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pip.UI.Migrations
{
    /// <inheritdoc />
    public partial class CleanupInvestmentModel : Migration
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
                value: null);

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuctionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Investments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AuctionDate",
                value: null);
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
