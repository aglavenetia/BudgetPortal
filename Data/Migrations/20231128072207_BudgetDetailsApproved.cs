using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class BudgetDetailsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetDetailsApproved",
                columns: table => new
                {
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    FinancialYear1 = table.Column<int>(type: "int", nullable: false),
                    FinancialYear2 = table.Column<int>(type: "int", nullable: false),
                    SectionNumber = table.Column<int>(type: "int", nullable: false),
                    GroupNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubGroupNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LedgerNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    BudEstCurrFinACandBW = table.Column<decimal>(type: "money", nullable: false),
                    RevEstCurrFinACandBW = table.Column<decimal>(type: "money", nullable: false),
                    BudEstNextFin = table.Column<decimal>(type: "money", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetailsApproved", x => new { x.DivisionID, x.FinancialYear1, x.FinancialYear2, x.SectionNumber, x.GroupNumber, x.SubGroupNumber, x.LedgerNumber });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetDetailsApproved");
        }
    }
}
