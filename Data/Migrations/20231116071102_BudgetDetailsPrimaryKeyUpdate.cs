using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class BudgetDetailsPrimaryKeyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BudgetDetails");

            migrationBuilder.AlterColumn<string>(
                name: "LedgerNumber",
                table: "BudgetDetails",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails",
                columns: new[] { "DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber", "SubGroupNumber", "LedgerNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails");

            migrationBuilder.AlterColumn<string>(
                name: "LedgerNumber",
                table: "BudgetDetails",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails",
                column: "Id");
        }
    }
}
