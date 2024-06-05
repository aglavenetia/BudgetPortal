using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class FewFieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACBWJustificationBudgEstNxtFin",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ACBWJustificationRevEst",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelegateJustificationRevEst",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACBWJustificationBudgEstNxtFin",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "ACBWJustificationRevEst",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "DelegateJustificationRevEst",
                table: "BudgetDetails");
        }
    }
}
