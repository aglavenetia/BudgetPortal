using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnFrombudgetledgers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireInput",
                table: "BudgetLedgers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireInput",
                table: "BudgetLedgers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
