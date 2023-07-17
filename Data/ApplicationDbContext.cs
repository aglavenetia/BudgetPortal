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

        [Key, ForeignKey("DealingHandID")]
        public int UserID { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SectionDetails> SectionDetails { get; set; }

        public  DbSet<SubSectionDetails> SubSectionDetails { get; set; }

        public DbSet<HeadDetails> HeadDetails { get; set; }

        public DbSet<SubHeadDetails> SubHeadDetails { get; set; }

        public DbSet<BudgetDetails> BudgetDetails { get; set;}

        public DbSet<Divisions> Divisions { get; set; }
    }
}