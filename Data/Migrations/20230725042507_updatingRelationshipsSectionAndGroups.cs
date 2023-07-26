using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatingRelationshipsSectionAndGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Sections_sectionsId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_sectionsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "sectionsId",
                table: "Groups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sectionsId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_sectionsId",
                table: "Groups",
                column: "sectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Sections_sectionsId",
                table: "Groups",
                column: "sectionsId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
