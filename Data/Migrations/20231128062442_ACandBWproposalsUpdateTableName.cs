using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ACandBWproposalsUpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ACandBWproposals",
                table: "ACandBWproposals");

            migrationBuilder.RenameTable(
                name: "ACandBWproposals",
                newName: "BudgetACandBWproposals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetACandBWproposals",
                table: "BudgetACandBWproposals",
                columns: new[] { "DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber", "SubGroupNumber", "LedgerNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetACandBWproposals",
                table: "BudgetACandBWproposals");

            migrationBuilder.RenameTable(
                name: "BudgetACandBWproposals",
                newName: "ACandBWproposals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ACandBWproposals",
                table: "ACandBWproposals",
                columns: new[] { "DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber", "SubGroupNumber", "LedgerNumber" });
        }
    }
}
