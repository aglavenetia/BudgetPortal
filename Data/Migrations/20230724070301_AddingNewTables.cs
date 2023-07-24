using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "LedgerDetails");

            migrationBuilder.DropTable(
                name: "SubGroupDetails");

            migrationBuilder.DropTable(
                name: "GroupDetails");

            migrationBuilder.DropTable(
                name: "SectionDetails");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "SubGroupNumberSubGroupNo",
                table: "BudgetDetails");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sections",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DivisionID",
                table: "BudgetDetails",
                newName: "DivisionId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BudgetDetails",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetDetails_DivisionID",
                table: "BudgetDetails",
                newName: "IX_BudgetDetails_DivisionId");

            migrationBuilder.AlterColumn<string>(
                name: "SectionName",
                table: "Sections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DivisionId",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubGroupNumberId",
                table: "BudgetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocumentPath",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionID = table.Column<int>(type: "int", nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DivisionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DealingHandID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupNo = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    sectionsId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "SubGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubGroupNo = table.Column<int>(type: "int", nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    groupsId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGroups_Groups_groupsId",
                        column: x => x.groupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LedgerNo = table.Column<int>(type: "int", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    subGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_SubGroups_subGroupsId",
                        column: x => x.subGroupsId,
                        principalTable: "SubGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_SubGroupNumberId",
                table: "BudgetDetails",
                column: "SubGroupNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_sectionsId",
                table: "Groups",
                column: "sectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_subGroupsId",
                table: "Ledgers",
                column: "subGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGroups_groupsId",
                table: "SubGroups",
                column: "groupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Division_DivisionId",
                table: "BudgetDetails",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_SubGroups_SubGroupNumberId",
                table: "BudgetDetails",
                column: "SubGroupNumberId",
                principalTable: "SubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Division_DivisionId",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_SubGroups_SubGroupNumberId",
                table: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "SubGroups");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDetails_SubGroupNumberId",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "SubGroupNumberId",
                table: "BudgetDetails");

            migrationBuilder.DropColumn(
                name: "SupportingDocumentPath",
                table: "BudgetDetails");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sections",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DivisionId",
                table: "BudgetDetails",
                newName: "DivisionID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BudgetDetails",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetDetails_DivisionId",
                table: "BudgetDetails",
                newName: "IX_BudgetDetails_DivisionID");

            migrationBuilder.AlterColumn<string>(
                name: "SectionName",
                table: "Sections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "BudgetDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DivisionID",
                table: "BudgetDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealingHandID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DivisionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DivisionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionID);
                });

            migrationBuilder.CreateTable(
                name: "SectionDetails",
                columns: table => new
                {
                    SectionNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetails", x => x.SectionNo);
                });

            migrationBuilder.CreateTable(
                name: "GroupDetails",
                columns: table => new
                {
                    GroupNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sectionsid = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionDetailsSectionNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetails", x => x.GroupNo);
                    table.ForeignKey(
                        name: "FK_GroupDetails_SectionDetails_SectionDetailsSectionNo",
                        column: x => x.SectionDetailsSectionNo,
                        principalTable: "SectionDetails",
                        principalColumn: "SectionNo");
                    table.ForeignKey(
                        name: "FK_GroupDetails_Sections_sectionsid",
                        column: x => x.sectionsid,
                        principalTable: "Sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGroupDetails",
                columns: table => new
                {
                    SubGroupNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupsGroupNo = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false),
                    subGroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroupDetails", x => x.SubGroupNo);
                    table.ForeignKey(
                        name: "FK_SubGroupDetails_GroupDetails_GroupsGroupNo",
                        column: x => x.GroupsGroupNo,
                        principalTable: "GroupDetails",
                        principalColumn: "GroupNo");
                });

            migrationBuilder.CreateTable(
                name: "LedgerDetails",
                columns: table => new
                {
                    LedgerNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subGroupsSubGroupNo = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_BudgetDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                column: "SubGroupNumberSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_SectionDetailsSectionNo",
                table: "GroupDetails",
                column: "SectionDetailsSectionNo");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_sectionsid",
                table: "GroupDetails",
                column: "sectionsid");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerDetails_subGroupsSubGroupNo",
                table: "LedgerDetails",
                column: "subGroupsSubGroupNo");

            migrationBuilder.CreateIndex(
                name: "IX_SubGroupDetails_GroupsGroupNo",
                table: "SubGroupDetails",
                column: "GroupsGroupNo");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Divisions_DivisionID",
                table: "BudgetDetails",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "DivisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_SubGroupDetails_SubGroupNumberSubGroupNo",
                table: "BudgetDetails",
                column: "SubGroupNumberSubGroupNo",
                principalTable: "SubGroupDetails",
                principalColumn: "SubGroupNo");
        }
    }
}
