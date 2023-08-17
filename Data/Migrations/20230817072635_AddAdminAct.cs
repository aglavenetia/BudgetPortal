using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAct : Migration
    {

        const string ADMIN_USER_GUID = "225d7a72-90cd-496f-8d57-02724ac91289";
        const string ADMIN_ROLE_GUID = "40692ebc-50b6-440f-8b4e-bb27e0bdc4ea";
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "BudgetPortal@123");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO AspNetUsers(Id,UserName,NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed," +
                "TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp,BranchName," +
                "FirstName,LastName)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");
            sb.AppendLine(",'admin@test.com'");
            sb.AppendLine(",'ADMIN@TEST.COM'");
            sb.AppendLine(",'admin@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'ADMIN@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'Accounts'");
            sb.AppendLine(",'AdminFirstName'");
            sb.AppendLine(",'AdminLastName'");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());

            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES('{ADMIN_ROLE_GUID}','Admin','Admin')");

            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId,RoleId) VALUES('{ADMIN_USER_GUID}','{ADMIN_ROLE_GUID}')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId='{ADMIN_USER_GUID}' AND RoleId ='{ADMIN_ROLE_GUID}' ");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{ADMIN_USER_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id ='{ADMIN_ROLE_GUID}'");
        }
    }
}
