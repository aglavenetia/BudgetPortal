using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetPortal.Entities;

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
        public DbSet<SectionDetails> SectionDetails { get; set; }
        public DbSet<GroupDetails> GroupDetails { get; set; }
        public DbSet<SubGroupDetails> SubGroupDetails { get; set; }

        public DbSet<LedgerDetails> LedgerDetails { get; set; }

        public DbSet<BudgetDetails> BudgetDetails { get; set; }

        public DbSet<Divisions> Divisions { get; set; }

    }


}