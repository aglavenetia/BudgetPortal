using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_CreatedDateTime_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                table: "SubGroupDetails");

            migrationBuilder.AlterColumn<string>(
                name: "subGroupName",
                table: "SubGroupDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "GroupsGroupNo",
                table: "SubGroupDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "SubGroupDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SectionName",
                table: "SectionDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "SectionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "LedgerDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "sectionsSectionNo",
                table: "GroupDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "GroupDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "GroupDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DivisionType",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DivisionName",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DealingHandID",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Divisions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DivisionID",
                table: "BudgetDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "BudgetDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "DivisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                column: "SubGroupNumberSubGroupNo",
                principalTable: "SubGroupDetails",
                principalColumn: "SubGroupNo");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails",
                column: "sectionsSectionNo",
                principalTable: "SectionDetails",
                principalColumn: "SectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                table: "SubGroupDetails",
                column: "GroupsGroupNo",
                principalTable: "GroupDetails",
                principalColumn: "GroupNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                table: "SubGroupDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "SubGroupDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "SectionDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "LedgerDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "GroupDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "BudgetDetails");

            migrationBuilder.AlterColumn<string>(
                name: "subGroupName",
                table: "SubGroupDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupsGroupNo",
                table: "SubGroupDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SectionName",
                table: "SectionDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sectionsSectionNo",
                table: "GroupDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "GroupDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DivisionType",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DivisionName",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DealingHandID",
                table: "Divisions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DivisionID",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "DivisionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                column: "SubGroupNumberSubGroupNo",
                principalTable: "SubGroupDetails",
                principalColumn: "SubGroupNo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails",
                column: "sectionsSectionNo",
                principalTable: "SectionDetails",
                principalColumn: "SectionNo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                table: "SubGroupDetails",
                column: "GroupsGroupNo",
                principalTable: "GroupDetails",
                principalColumn: "GroupNo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
