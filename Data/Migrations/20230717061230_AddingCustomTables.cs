using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    public partial class AddingCustomTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DivID = table.Column<int>(type: "int", nullable: false),
                    FinancialYear1 = table.Column<int>(type: "int", nullable: false),
                    FinancialYear2 = table.Column<int>(type: "int", nullable: false),
                    HeadNumber = table.Column<int>(type: "int", nullable: false),
                    SubHeadNumber = table.Column<int>(type: "int", nullable: false),
                    BudEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    ActPrevFin = table.Column<decimal>(type: "money", nullable: false),
                    ActCurrFinTill2ndQuart = table.Column<decimal>(type: "money", nullable: false),
                    RevEstCurrFin = table.Column<decimal>(type: "money", nullable: false),
                    PerVarRevEstOverBudgEstCurrFin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ACAndBWPropRECurrFin = table.Column<decimal>(type: "money", nullable: false),
                    BudgEstNexFin = table.Column<decimal>(type: "money", nullable: false),
                    PerVarRevEstOverBudgEstNxtFin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ACAndBWPropRENxtFin = table.Column<decimal>(type: "money", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivisionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DealingHandID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Divisions_BudgetDetails_DivID",
                        column: x => x.DivID,
                        principalTable: "BudgetDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SubHeadDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadNumber = table.Column<int>(type: "int", nullable: false),
                    SubHeadNo = table.Column<int>(type: "int", nullable: false),
                    SubHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubHeadNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHeadDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubHeadDetails_BudgetDetails_SubHeadNumber",
                        column: x => x.SubHeadNumber,
                        principalTable: "BudgetDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HeadDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadNo = table.Column<int>(type: "int", nullable: false),
                    HeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubSectionNumber = table.Column<int>(type: "int", nullable: false),
                    HeadNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_HeadDetails_BudgetDetails_HeadNumber",
                        column: x => x.HeadNumber,
                        principalTable: "BudgetDetails",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_HeadDetails_SubHeadDetails_HeadNumber",
                        column: x => x.HeadNumber,
                        principalTable: "SubHeadDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SubSectionDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionNumber = table.Column<int>(type: "int", nullable: false),
                    SubSectionNo = table.Column<int>(type: "int", nullable: false),
                    SubSectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubSectionNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSectionDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubSectionDetails_HeadDetails_SubSectionNumber",
                        column: x => x.SubSectionNumber,
                        principalTable: "HeadDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SectionDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionNo = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_SectionDetails_SubSectionDetails_SectionNumber",
                        column: x => x.SectionNumber,
                        principalTable: "SubSectionDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_DivID",
                table: "Divisions",
                column: "DivID");

            migrationBuilder.CreateIndex(
                name: "IX_HeadDetails_HeadNumber",
                table: "HeadDetails",
                column: "HeadNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDetails_SectionNumber",
                table: "SectionDetails",
                column: "SectionNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SubHeadDetails_SubHeadNumber",
                table: "SubHeadDetails",
                column: "SubHeadNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SubSectionDetails_SubSectionNumber",
                table: "SubSectionDetails",
                column: "SubSectionNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "UserID",
                table: "AspNetUsers");
        }
    }
}
