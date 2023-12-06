using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatingPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetdetailsStatus",
                table: "BudgetdetailsStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetdetailsStatus",
                table: "BudgetdetailsStatus",
                columns: new[] { "DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetdetailsStatus",
                table: "BudgetdetailsStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetdetailsStatus",
                table: "BudgetdetailsStatus",
                columns: new[] { "DivisionID", "FinancialYear1", "FinancialYear2" });
        }
    }
}
