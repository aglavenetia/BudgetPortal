using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccount4COEEastDelhi : Migration
    {
        const string DELEGATE_USER_GUID = "74166e83-4c79-468b-855d-3aab79fb53b1";
        /// <inheritdoc />
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
            sb.AppendLine(",'COEEastDelhi@test.com'");
            sb.AppendLine(",'COEEastDelhi@TEST.COM'");
            sb.AppendLine(",'COEEastDelhi@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'COEEastDelhi@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'COE EastDelhi'");
            sb.AppendLine(",'COEEastDelhiFirstName'");
            sb.AppendLine(",'COEEastDelhiLastName'");
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
