using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    public partial class redefiningtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivisionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DealingHandID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionID);
                });

            migrationBuilder.CreateTable(
                name: "SubGroupDetails",
                columns: table => new
                {
                    SubGroupNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupsGroupNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroupDetails", x => x.SubGroupNo);
                    table.ForeignKey(
                        name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                        column: x => x.GroupsGroupNo,
                        principalTable: "GroupDetails",
                        principalColumn: "GroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    FinancialYear1 = table.Column<int>(type: "int", nullable: false),
                    FinancialYear2 = table.Column<int>(type: "int", nullable: false),
                    SubGroupNumberSubGroupNo = table.Column<int>(type: "int", nullable: false),
                    BudEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    ActPrevFin = table.Column<decimal>(type: "money", nullable: false),
                    ActCurrFinTill2ndQuart = table.Column<decimal>(type: "money", nullable: false),
                    RevEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    PerVarRevEstOverBudgEstCurrFin = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ACAndBWPropRECurrFin = table.Column<decimal>(type: "money", nullable: false),
                    BudgEstNexFin = table.Column<decimal>(type: "money", nullable: false),
                    PerVarRevEstOverBudgEstNxtFin = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ACAndBWPropRENxtFin = table.Column<decimal>(type: "money", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "DivisionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                        column: x => x.SubGroupNumberSubGroupNo,
                        principalTable: "SubGroupDetails",
                        principalColumn: "SubGroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LedgerDetails",
                columns: table => new
                {
                    LedgerNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    subGroupsSubGroupNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerDetails", x => x.LedgerNo);
                    table.ForeignKey(
                        name: "FK_LedgerDetails_SubGroupDetails_subGroupsSubGroupNo",
                        column: x => x.subGroupsSubGroupNo,
                        principalTable: "SubGroupDetails",
                        principalColumn: "SubGroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_DivisionID",
                table: "BudgetDetails",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                column: "SubGroupNumberSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerDetails_subGroupsSubGroupNo",
                table: "LedgerDetails",
                column: "subGroupsSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_SubGroupDetails_GroupsGroupNo",
                table: "SubGroupDetails",
                column: "GroupsGroupNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "LedgerDetails");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "SubGroupDetails");
        }
    }
}
