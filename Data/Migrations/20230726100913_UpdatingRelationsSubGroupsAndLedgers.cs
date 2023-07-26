using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingRelationsSubGroupsAndLedgers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.CreateTable(
                name: "BudgetLedgers",
                columns: table => new
                {
                    LedgerNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubGroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLedgers", x => x.LedgerNo);
                    table.ForeignKey(
                        name: "FK_BudgetLedgers_BudgetSubGroups_SubGroupNo",
                        column: x => x.SubGroupNo,
                        principalTable: "BudgetSubGroups",
                        principalColumn: "SubGroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLedgers_SubGroupNo",
                table: "BudgetLedgers",
                column: "SubGroupNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLedgers");

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetSubGroupsSubGroupNo = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LedgerNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubGroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_BudgetSubGroups_BudgetSubGroupsSubGroupNo",
                        column: x => x.BudgetSubGroupsSubGroupNo,
                        principalTable: "BudgetSubGroups",
                        principalColumn: "SubGroupNo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_BudgetSubGroupsSubGroupNo",
                table: "Ledgers",
                column: "BudgetSubGroupsSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerNo",
                table: "Ledgers",
                column: "LedgerNo",
                unique: true);
        }
    }
}
