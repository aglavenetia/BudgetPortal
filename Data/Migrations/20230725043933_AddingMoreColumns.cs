using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Division_DivisionId",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_SubGroups_SubGroupNumberId",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_SubGroups_subGroupsId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubGroups_Groups_groupsId",
                table: "SubGroups");

            migrationBuilder.DropIndex(
                name: "IX_SubGroups_groupsId",
                table: "SubGroups");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_subGroupsId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDetails_DivisionId",
                table: "BudgetDetails");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDetails_SubGroupNumberId",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "groupsId",
                table: "SubGroups");

            migrationBuilder.DropColumn(
                name: "subGroupsId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "LedgerNo",
                table: "BudgetDetails");

            migrationBuilder.RenameColumn(
                name: "DivisionId",
                table: "BudgetDetails",
                newName: "DivisionID");

            migrationBuilder.RenameColumn(
                name: "SubGroupNumberId",
                table: "BudgetDetails",
                newName: "SectionNumber");

            migrationBuilder.AlterColumn<string>(
                name: "SubGroupNo",
                table: "SubGroups",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GroupNo",
                table: "SubGroups",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LedgerNo",
                table: "Ledgers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SubGroupNo",
                table: "Ledgers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "GroupNo",
                table: "Groups",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GroupNumber",
                table: "BudgetDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LedgerNumber",
                table: "BudgetDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubGroupNumber",
                table: "BudgetDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupNo",
                table: "SubGroups");

            migrationBuilder.DropColumn(
                name: "SubGroupNo",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "GroupNumber",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "LedgerNumber",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "SubGroupNumber",
                table: "BudgetDetails");

            migrationBuilder.RenameColumn(
                name: "DivisionID",
                table: "BudgetDetails",
                newName: "DivisionId");

            migrationBuilder.RenameColumn(
                name: "SectionNumber",
                table: "BudgetDetails",
                newName: "SubGroupNumberId");

            migrationBuilder.AlterColumn<int>(
                name: "SubGroupNo",
                table: "SubGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "groupsId",
                table: "SubGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "LedgerNo",
                table: "Ledgers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "subGroupsId",
                table: "Ledgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GroupNo",
                table: "Groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "LedgerNo",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubGroups_groupsId",
                table: "SubGroups",
                column: "groupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_subGroupsId",
                table: "Ledgers",
                column: "subGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_DivisionId",
                table: "BudgetDetails",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_SubGroupNumberId",
                table: "BudgetDetails",
                column: "SubGroupNumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Division_DivisionId",
                table: "BudgetDetails",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_SubGroups_SubGroupNumberId",
                table: "BudgetDetails",
                column: "SubGroupNumberId",
                principalTable: "SubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_SubGroups_subGroupsId",
                table: "Ledgers",
                column: "subGroupsId",
                principalTable: "SubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubGroups_Groups_groupsId",
                table: "SubGroups",
                column: "groupsId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
