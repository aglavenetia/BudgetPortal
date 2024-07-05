using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingDetailsToBudgetDetailsStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ACBWSubmission",
                table: "BudgetdetailsStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ChairpersonApproval",
                table: "BudgetdetailsStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FinCommitteeApproval",
                table: "BudgetdetailsStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GenBodyApproval",
                table: "BudgetdetailsStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACBWSubmission",
                table: "BudgetdetailsStatus");

            migrationBuilder.DropColumn(
                name: "ChairpersonApproval",
                table: "BudgetdetailsStatus");

            migrationBuilder.DropColumn(
                name: "FinCommitteeApproval",
                table: "BudgetdetailsStatus");

            migrationBuilder.DropColumn(
                name: "GenBodyApproval",
                table: "BudgetdetailsStatus");
        }
    }
}
