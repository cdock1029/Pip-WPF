using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pip.UI.Migrations
{
    /// <inheritdoc />
    public partial class IndexOnInvestment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Investments_Cusip_IssueDate",
                table: "Investments",
                columns: new[] { "Cusip", "IssueDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investments_Cusip_IssueDate",
                table: "Investments");
        }
    }
}
