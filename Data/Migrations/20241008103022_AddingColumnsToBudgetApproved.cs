using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingColumnsToBudgetApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EstIncreasedBy",
                table: "BudgetDetailsApproved",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalEstimates",
                table: "BudgetDetailsApproved",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstIncreasedBy",
                table: "BudgetDetailsApproved");

            migrationBuilder.DropColumn(
                name: "FinalEstimates",
                table: "BudgetDetailsApproved");
        }
    }
}
