using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakingNotNull1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_BudgetSections_sectionsSectionNo",
                table: "BudgetGroups");

            migrationBuilder.RenameColumn(
                name: "sectionsSectionNo",
                table: "BudgetGroups",
                newName: "SectionNo");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetGroups_sectionsSectionNo",
                table: "BudgetGroups",
                newName: "IX_BudgetGroups_SectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_BudgetSections_SectionNo",
                table: "BudgetGroups",
                column: "SectionNo",
                principalTable: "BudgetSections",
                principalColumn: "SectionNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_BudgetSections_SectionNo",
                table: "BudgetGroups");

            migrationBuilder.RenameColumn(
                name: "SectionNo",
                table: "BudgetGroups",
                newName: "sectionsSectionNo");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetGroups_SectionNo",
                table: "BudgetGroups",
                newName: "IX_BudgetGroups_sectionsSectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_BudgetSections_sectionsSectionNo",
                table: "BudgetGroups",
                column: "sectionsSectionNo",
                principalTable: "BudgetSections",
                principalColumn: "SectionNo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
