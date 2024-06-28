using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingColumnsToBudgetDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PerVarACBWRevEstOverBudgEstCurrFin",
                table: "BudgetDetails",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PerVarACBWRevEstOverBudgEstNxtFin",
                table: "BudgetDetails",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerVarACBWRevEstOverBudgEstCurrFin",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "PerVarACBWRevEstOverBudgEstNxtFin",
                table: "BudgetDetails");
        }
    }
}
