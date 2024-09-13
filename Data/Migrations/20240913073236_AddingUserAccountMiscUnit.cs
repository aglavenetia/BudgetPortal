using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAccountMiscUnit : Migration
    {
        const string DELEGATE_USER_GUID = "36cb3bac-487a-4f34-ba5a-8158d6e1db42";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "User@123");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO AspNetUsers(Id,UserName,NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed," +
                "TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp,BranchName," +
                "FirstName,LastName)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{DELEGATE_USER_GUID}'");
            sb.AppendLine(",'MiscUnit@test.com'");
            sb.AppendLine(",'MiscUnit@TEST.COM'");
            sb.AppendLine(",'MiscUnit@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'MiscUnit@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'Misc Unit'");
            sb.AppendLine(",'MiscUnitFirstName'");
            sb.AppendLine(",'MiscUnitLastName'");
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
