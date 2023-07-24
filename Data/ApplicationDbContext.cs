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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sections>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Sections>()
                .HasIndex(Sections => new { Sections.SectionNo }).IsUnique();

            modelBuilder.Entity<Groups>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Groups>()
                .HasIndex(Groups => new { Groups.GroupNo }).IsUnique();

            modelBuilder.Entity<SubGroups>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<SubGroups>()
                .HasIndex(SubGroups => new { SubGroups.SubGroupNo }).IsUnique();

            modelBuilder.Entity<Ledgers>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Ledgers>()
                .HasIndex(Ledgers => new { Ledgers.LedgerNo }).IsUnique();

            modelBuilder.Entity<BudgetDetails>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Division>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<Sections> Sections { get; set; }

        public DbSet<Groups> Groups { get; set; }
        public DbSet<SubGroups> SubGroups { get; set; }
        public DbSet<Ledgers> Ledgers { get; set; }
        public DbSet<Division> Division { get; set; }

        public DbSet<BudgetDetails> BudgetDetails { get; set; }
    }


}