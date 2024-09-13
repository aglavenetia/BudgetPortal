using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAccountROWestDelhi : Migration
    {
        const string DELEGATE_USER_GUID = "a44dce79-6f54-4470-b90f-c47116c0c948";
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
            sb.AppendLine(",'ROWestDelhi@test.com'");
            sb.AppendLine(",'ROWestDelhi@TEST.COM'");
            sb.AppendLine(",'ROWestDelhi@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'ROWestDelhi@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'RO ROWestDelhi'");
            sb.AppendLine(",'ROWestDelhiFirstName'");
            sb.AppendLine(",'ROWestDelhiLastName'");
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
