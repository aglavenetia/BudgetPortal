using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLedgers_BudgetSubGroups_SubGroupNo",
                table: "BudgetLedgers");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLedgers_SubGroupNo",
                table: "BudgetLedgers");

            migrationBuilder.AlterColumn<string>(
                name: "SubGroupNo",
                table: "BudgetLedgers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "subGroupsSubGroupNo",
                table: "BudgetLedgers",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLedgers_subGroupsSubGroupNo",
                table: "BudgetLedgers",
                column: "subGroupsSubGroupNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLedgers_BudgetSubGroups_subGroupsSubGroupNo",
                table: "BudgetLedgers",
                column: "subGroupsSubGroupNo",
                principalTable: "BudgetSubGroups",
                principalColumn: "SubGroupNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLedgers_BudgetSubGroups_subGroupsSubGroupNo",
                table: "BudgetLedgers");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLedgers_subGroupsSubGroupNo",
                table: "BudgetLedgers");

            migrationBuilder.DropColumn(
                name: "subGroupsSubGroupNo",
                table: "BudgetLedgers");

            migrationBuilder.AlterColumn<string>(
                name: "SubGroupNo",
                table: "BudgetLedgers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLedgers_SubGroupNo",
                table: "BudgetLedgers",
                column: "SubGroupNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLedgers_BudgetSubGroups_SubGroupNo",
                table: "BudgetLedgers",
                column: "SubGroupNo",
                principalTable: "BudgetSubGroups",
                principalColumn: "SubGroupNo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
