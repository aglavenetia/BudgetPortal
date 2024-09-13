using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAccountROEastDelhi : Migration
    {
        /// <inheritdoc />
        const string DELEGATE_USER_GUID = "0acf39a4-3c46-479e-9539-fcb1cf715a4e";
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
            sb.AppendLine(",'ROEastDelhi@test.com'");
            sb.AppendLine(",'ROEastDelhi@TEST.COM'");
            sb.AppendLine(",'ROEastDelhi@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'ROEastDelhi@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'RO EastDelhi'");
            sb.AppendLine(",'ROEastDelhiFirstName'");
            sb.AppendLine(",'ROEastDelhiLastName'");
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
