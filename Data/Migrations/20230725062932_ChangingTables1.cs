using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangingTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.CreateTable(
                name: "SectionDetails",
                columns: table => new
                {
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetails", x => x.SectionNo);
                });

            migrationBuilder.CreateTable(
                name: "GroupDetails",
                columns: table => new
                {
                    GroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionNo = table.Column<int>(type: "int", nullable: false),
                    sectionsSectionNo = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetails", x => x.GroupNo);
                    table.ForeignKey(
                        name: "FK_GroupDetails_SectionDetails_sectionsSectionNo",
                        column: x => x.sectionsSectionNo,
                        principalTable: "SectionDetails",
                        principalColumn: "SectionNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_GroupNo",
                table: "GroupDetails",
                column: "GroupNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_sectionsSectionNo",
                table: "GroupDetails",
                column: "sectionsSectionNo");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDetails_SectionNo",
                table: "SectionDetails",
                column: "SectionNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDetails");

            migrationBuilder.DropTable(
                name: "SectionDetails");

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sectionsId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Sections_sectionsId",
                        column: x => x.sectionsId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupNo",
                table: "Groups",
                column: "GroupNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_sectionsId",
                table: "Groups",
                column: "sectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionNo",
                table: "Sections",
                column: "SectionNo",
                unique: true);
        }
    }
}
