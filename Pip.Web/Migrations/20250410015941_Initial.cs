using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pip.Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cusip = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Par = table.Column<int>(type: "int", nullable: false),
                    MaturityDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AuctionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Confirmation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reinvestments = table.Column<int>(type: "int", nullable: false),
                    SecurityTerm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Investments",
                columns: new[] { "Id", "AuctionDate", "Confirmation", "Cusip", "IssueDate", "MaturityDate", "Par", "Reinvestments", "SecurityTerm", "Type" },
                values: new object[,]
                {
                    { 1, null, "FOO", "912797GL5", new DateOnly(2024, 7, 25), new DateOnly(2024, 9, 5), 100000, 0, "42-Day", 0 },
                    { 2, null, "BAR", "912797KX4", new DateOnly(2024, 6, 18), new DateOnly(2024, 8, 13), 55000, 0, "8-Week", 0 },
                    { 3, null, "BAZ", "912797GK7", new DateOnly(2024, 5, 9), new DateOnly(2024, 8, 8), 2000400, 0, "13-Week", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investments_Cusip_IssueDate",
                table: "Investments",
                columns: new[] { "Cusip", "IssueDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments");
        }
    }
}
