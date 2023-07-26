using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UPdatingTableRelationsGroupAndSubGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubGroups");

            migrationBuilder.AddColumn<string>(
                name: "BudgetSubGroupsSubGroupNo",
                table: "Ledgers",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BudgetSubGroups",
                columns: table => new
                {
                    SubGroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupNo = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetSubGroups", x => x.SubGroupNo);
                    table.ForeignKey(
                        name: "FK_BudgetSubGroups_BudgetGroups_GroupNo",
                        column: x => x.GroupNo,
                        principalTable: "BudgetGroups",
                        principalColumn: "GroupNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_BudgetSubGroupsSubGroupNo",
                table: "Ledgers",
                column: "BudgetSubGroupsSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetSubGroups_GroupNo",
                table: "BudgetSubGroups",
                column: "GroupNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_BudgetSubGroups_BudgetSubGroupsSubGroupNo",
                table: "Ledgers",
                column: "BudgetSubGroupsSubGroupNo",
                principalTable: "BudgetSubGroups",
                principalColumn: "SubGroupNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_BudgetSubGroups_BudgetSubGroupsSubGroupNo",
                table: "Ledgers");

            migrationBuilder.DropTable(
                name: "BudgetSubGroups");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_BudgetSubGroupsSubGroupNo",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "BudgetSubGroupsSubGroupNo",
                table: "Ledgers");

            migrationBuilder.CreateTable(
                name: "SubGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    GroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubGroupNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubGroups_SubGroupNo",
                table: "SubGroups",
                column: "SubGroupNo",
                unique: true);
        }
    }
}
