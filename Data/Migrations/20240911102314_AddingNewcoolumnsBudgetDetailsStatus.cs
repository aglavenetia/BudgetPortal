using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewcoolumnsBudgetDetailsStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalComments",
                table: "BudgetdetailsStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeadApproved",
                table: "BudgetdetailsStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "eoffFileNo",
                table: "BudgetdetailsStatus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalComments",
                table: "BudgetdetailsStatus");

            migrationBuilder.DropColumn(
                name: "IsHeadApproved",
                table: "BudgetdetailsStatus");

            migrationBuilder.DropColumn(
                name: "eoffFileNo",
                table: "BudgetdetailsStatus");
        }
    }
}
