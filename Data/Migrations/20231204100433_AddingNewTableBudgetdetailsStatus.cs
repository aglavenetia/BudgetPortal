using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewTableBudgetdetailsStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetdetailsStatus",
                columns: table => new
                {
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    FinancialYear1 = table.Column<int>(type: "int", nullable: false),
                    FinancialYear2 = table.Column<int>(type: "int", nullable: false),
                    DelegateEditStatus = table.Column<bool>(type: "bit", nullable: false),
                    AdminEditStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetdetailsStatus", x => new { x.DivisionID, x.FinancialYear1, x.FinancialYear2 });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetdetailsStatus");
        }
    }
}
