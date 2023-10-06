using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccount1 : Migration
    {
        /// <inheritdoc />
        /// 
        const string DELEGATE_USER_GUID = "91fc12fa-6cee-4997-8159-c6eae695364d";
        //const string DELEGATE_ROLE_GUID = "f1143895-a01c-44f3-987e-15295abb3640";
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
            sb.AppendLine(",'UnitB@test.com'");
            sb.AppendLine(",'UNITB@TEST.COM'");
            sb.AppendLine(",'unitb@test.com'");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",0");
            sb.AppendLine(",'UNITB@TEST.COM'");
            sb.AppendLine($",'{passwordHash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",'UnitB'");
            sb.AppendLine(",'UnitBFirstName'");
            sb.AppendLine(",'UnitBLastName'");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());

            //migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES('{DELEGATE_ROLE_GUID}','Delegate','Delegate')");

           // migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId,RoleId) VALUES('{DELEGATE_USER_GUID}','{DELEGATE_ROLE_GUID}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId='{DELEGATE_USER_GUID}' AND RoleId ='{DELEGATE_ROLE_GUID}' ");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{DELEGATE_USER_GUID}'");

            //migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id ='{DELEGATE_ROLE_GUID}'");
        }
    }
}
