using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pip.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SqliteOnlyInvestment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cusip = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Par = table.Column<int>(type: "INTEGER", nullable: false),
                    Confirmation = table.Column<string>(type: "TEXT", nullable: true),
                    Reinvestments = table.Column<int>(type: "INTEGER", nullable: false),
                    MaturityDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    SecurityTerm = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Investments",
                columns: new[] { "Id", "Confirmation", "Cusip", "IssueDate", "MaturityDate", "Par", "Reinvestments", "SecurityTerm", "Type" },
                values: new object[,]
                {
                    { 1, "FOO", "912797GL5", new DateOnly(2024, 7, 25), new DateOnly(2024, 9, 5), 100000, 0, "42-Day", 0 },
                    { 2, "BAR", "912797KX4", new DateOnly(2024, 6, 18), new DateOnly(2024, 8, 13), 55000, 0, "8-Week", 0 },
                    { 3, "BAZ", "912797GK7", new DateOnly(2024, 5, 9), new DateOnly(2024, 8, 8), 2000400, 0, "13-Week", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments");
        }
    }
}
