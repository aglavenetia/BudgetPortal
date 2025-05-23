﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetPortal.Entities;
using System.Reflection.Metadata;

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

            modelBuilder.Entity<BudgetSections>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");
     
            modelBuilder.Entity<BudgetSections>()
                .HasMany(e => e.Groups)
                .WithOne(e => e.Sections)
                .HasForeignKey(e => e.SectionNo)
                .IsRequired();

            modelBuilder.Entity<BudgetGroups>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetGroups>()
                .HasMany(e => e.SubGroups)
                .WithOne(e => e.groups)
                .HasForeignKey(e => e.GroupNo)
                .IsRequired();

            modelBuilder.Entity<BudgetLedgers>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetLedgers>()
                .HasMany(e => e.Ledgers)
                .WithOne(e => e.subGroups)
                .HasForeignKey(e => e.SubGroupNo)
                .IsRequired();

            modelBuilder.Entity<BudgetSubLedgers>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");   

            modelBuilder.Entity<BudgetDetails>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetDetailsApproved>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetdetailsStatus>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Division>()
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetdetailsStatus>()
          //      .ToTable(b => b.HasTrigger("trInsertFinalSubmitStatus"))
                .ToTable(b => b.HasTrigger("trUpdateFinalSubmitStatus"))
                .Property(b => b.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<BudgetFiles>()
                .Property(b => b.UploadedDateTime)
                .HasDefaultValueSql("GETDATE()");
        }

 

        public DbSet<BudgetSections> BudgetSections { get; set; }
        public DbSet<BudgetGroups> BudgetGroups { get; set; }
        public DbSet<BudgetLedgers> BudgetSubGroups { get; set; }
        public DbSet<BudgetSubLedgers> BudgetLedgers { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<BudgetDetails> BudgetDetails { get; set; }
        public DbSet<AcademicYears> AcademicYears { get; set; }
        public DbSet<BudgetReports> BudgetReports { get; set;}
        public DbSet<BudgetDetailsApproved> BudgetDetailsApproved { get; set; }
        public DbSet<BudgetdetailsStatus> BudgetdetailsStatus { get; set; }
        public DbSet<BudgetFiles> BudgetFiles { get; set; }
    }


}