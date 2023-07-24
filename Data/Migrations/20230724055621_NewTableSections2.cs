using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTableSections2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails");

            migrationBuilder.RenameColumn(
                name: "sectionsSectionNo",
                table: "GroupDetails",
                newName: "SectionDetailsSectionNo");

            migrationBuilder.RenameIndex(
                name: "IX_GroupDetails_sectionsSectionNo",
                table: "GroupDetails",
                newName: "IX_GroupDetails_SectionDetailsSectionNo");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "GroupDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sectionsid",
                table: "GroupDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionNo = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_sectionsid",
                table: "GroupDetails",
                column: "sectionsid");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupDetails_SectionDetails_SectionDetailsSectionNo",
                table: "GroupDetails",
                column: "SectionDetailsSectionNo",
                principalTable: "SectionDetails",
                principalColumn: "SectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupDetails_Sections_sectionsid",
                table: "GroupDetails",
                column: "sectionsid",
                principalTable: "Sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupDetails_SectionDetails_SectionDetailsSectionNo",
                table: "GroupDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupDetails_Sections_sectionsid",
                table: "GroupDetails");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_GroupDetails_sectionsid",
                table: "GroupDetails");

            migrationBuilder.DropColumn(
                name: "sectionsid",
                table: "GroupDetails");

            migrationBuilder.RenameColumn(
                name: "SectionDetailsSectionNo",
                table: "GroupDetails",
                newName: "sectionsSectionNo");

            migrationBuilder.RenameIndex(
                name: "IX_GroupDetails_SectionDetailsSectionNo",
                table: "GroupDetails",
                newName: "IX_GroupDetails_sectionsSectionNo");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "GroupDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "GroupDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                table: "GroupDetails",
                column: "sectionsSectionNo",
                principalTable: "SectionDetails",
                principalColumn: "SectionNo");
        }
    }
}
