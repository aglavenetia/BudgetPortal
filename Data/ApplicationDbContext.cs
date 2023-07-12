using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Data
{

    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public String BranchName { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}