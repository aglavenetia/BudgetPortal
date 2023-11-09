using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewColumnToSubGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireInput",
                table: "BudgetSubGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireInput",
                table: "BudgetSubGroups");
        }
    }
}
