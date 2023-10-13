using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingReportTableAndAColumnInBudgetDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BudgEstNexFinProposed",
                table: "BudgetDetails",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BudgetReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportID = table.Column<int>(type: "int", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetReports");

            migrationBuilder.DropColumn(
                name: "BudgEstNexFinProposed",
                table: "BudgetDetails");
        }
    }
}
