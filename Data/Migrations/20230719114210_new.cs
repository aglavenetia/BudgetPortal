using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "SectionDetails");

            migrationBuilder.DropTable(
                name: "SubSectionDetails");

            migrationBuilder.DropTable(
                name: "HeadDetails");

            migrationBuilder.DropTable(
                name: "SubHeadDetails");

            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "SecDetails",
                columns: table => new
                {
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecDetails", x => x.SectionNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecDetails");

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACAndBWPropRECurrFin = table.Column<decimal>(type: "money", nullable: false),
                    ACAndBWPropRENxtFin = table.Column<decimal>(type: "money", nullable: false),
                    ActCurrFinTill2ndQuart = table.Column<decimal>(type: "money", nullable: false),
                    ActPrevFin = table.Column<decimal>(type: "money", nullable: false),
                    BudEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    BudgEstNexFin = table.Column<decimal>(type: "money", nullable: false),
                    DivID = table.Column<int>(type: "int", nullable: false),
                    FinancialYear1 = table.Column<int>(type: "int", nullable: false),
                    FinancialYear2 = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerNumber = table.Column<int>(type: "int", nullable: false),
                    PerVarRevEstOverBudgEstCurrFin = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    PerVarRevEstOverBudgEstNxtFin = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    RevEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    SubGroupNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    DealingHandID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivisionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionID);
                    table.ForeignKey(
                        name: "FK_Divisions_BudgetDetails_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "BudgetDetails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LedgerDetails",
                columns: table => new
                {
                    LedgerNo = table.Column<int>(type: "int", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubGroupNumber = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerDetails", x => x.LedgerNo);
                    table.ForeignKey(
                        name: "FK_LedgerDetails_BudgetDetails_LedgerNo",
                        column: x => x.LedgerNo,
                        principalTable: "BudgetDetails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGroupDetails",
                columns: table => new
                {
                    SubGroupNo = table.Column<int>(type: "int", nullable: false),
                    GroupNumber = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroupDetails", x => x.SubGroupNo);
                    table.ForeignKey(
                        name: "FK_SubGroupDetails_BudgetDetails_SubGroupNo",
                        column: x => x.SubGroupNo,
                        principalTable: "BudgetDetails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubGroupDetails_LedgerDetails_SubGroupNo",
                        column: x => x.SubGroupNo,
                        principalTable: "LedgerDetails",
                        principalColumn: "LedgerNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupDetails",
                columns: table => new
                {
                    GroupNo = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionNumber = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetails", x => x.GroupNo);
                    table.ForeignKey(
                        name: "FK_GroupDetails_SubGroupDetails_GroupNo",
                        column: x => x.GroupNo,
                        principalTable: "SubGroupDetails",
                        principalColumn: "SubGroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionDetails",
                columns: table => new
                {
                    SectionNo = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetails", x => x.SectionNo);
                    table.ForeignKey(
                        name: "FK_SectionDetails_GroupDetails_SectionNo",
                        column: x => x.SectionNo,
                        principalTable: "GroupDetails",
                        principalColumn: "GroupNo",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
