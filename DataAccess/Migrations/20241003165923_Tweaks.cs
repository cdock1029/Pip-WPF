using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pip.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Tweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefCPIOnDatedDate",
                table: "Treasuries",
                newName: "RefCpiOnDatedDate");

            migrationBuilder.RenameColumn(
                name: "CashManagementBillCMB",
                table: "Treasuries",
                newName: "CashManagementBillCmb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefCpiOnDatedDate",
                table: "Treasuries",
                newName: "RefCPIOnDatedDate");

            migrationBuilder.RenameColumn(
                name: "CashManagementBillCmb",
                table: "Treasuries",
                newName: "CashManagementBillCMB");
        }
    }
}
