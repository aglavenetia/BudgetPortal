using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    public partial class SectionDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectionDetails",
                columns: table => new
                {
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetails", x => x.SectionNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionDetails");
        }
    }
}
