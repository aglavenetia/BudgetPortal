using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatingTableRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_BudgetSections_SectionNo",
                table: "BudgetGroups");

            migrationBuilder.DropIndex(
                name: "IX_BudgetGroups_SectionNo",
                table: "BudgetGroups");

            migrationBuilder.AddColumn<int>(
                name: "sectionsSectionNo",
                table: "BudgetGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroups_sectionsSectionNo",
                table: "BudgetGroups",
                column: "sectionsSectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_BudgetSections_sectionsSectionNo",
                table: "BudgetGroups",
                column: "sectionsSectionNo",
                principalTable: "BudgetSections",
                principalColumn: "SectionNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetGroups_BudgetSections_sectionsSectionNo",
                table: "BudgetGroups");

            migrationBuilder.DropIndex(
                name: "IX_BudgetGroups_sectionsSectionNo",
                table: "BudgetGroups");

            migrationBuilder.DropColumn(
                name: "sectionsSectionNo",
                table: "BudgetGroups");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroups_SectionNo",
                table: "BudgetGroups",
                column: "SectionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroups_BudgetSections_SectionNo",
                table: "BudgetGroups",
                column: "SectionNo",
                principalTable: "BudgetSections",
                principalColumn: "SectionNo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
