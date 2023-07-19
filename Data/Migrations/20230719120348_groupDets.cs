using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    public partial class groupDets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "GroupDetails",
                columns: table => new
                {
                    GroupNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetails", x => x.GroupNo);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SectionDetails_GroupDetails_SectionNo",
                table: "SectionDetails",
                column: "SectionNo",
                principalTable: "GroupDetails",
                principalColumn: "GroupNo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionDetails_GroupDetails_SectionNo",
                table: "SectionDetails");

            migrationBuilder.DropTable(
                name: "GroupDetails");

            
        }
    }
}
