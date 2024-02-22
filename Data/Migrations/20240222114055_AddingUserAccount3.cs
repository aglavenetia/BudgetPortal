using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAccount3 : Migration
    {
        /// <inheritdoc />
        const string DELEGATE_USER_GUID = "fca3fa19-9633-4963-af6c-23be195230cc";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "BudgetPortalUser@123");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO AspNetUsers(Id,UserName,NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed," +
                "TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp,BranchName," +
                "FirstName,LastName)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{DELEGATE_USER_GUID}'");
            sb.AppendLine(",'UnitC@test.com'");
            sb.AppendLine(",'UNITC@TEST.COM'");
            sb.AppendLine(",'unitc@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'UNITC@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'COE Patna'");
            sb.AppendLine(",'UnitCFirstName'");
            sb.AppendLine(",'UnitCLastName'");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{DELEGATE_USER_GUID}'");
        }
    }
}
