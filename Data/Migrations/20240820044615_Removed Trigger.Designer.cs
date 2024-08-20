﻿// <auto-generated />
using System;
using BudgetPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BudgetPortal.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240820044615_Removed Trigger")]
    partial class RemovedTrigger
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BudgetPortal.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BudgetPortal.Entities.AcademicYears", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Year1")
                        .HasColumnType("int");

                    b.Property<int>("Year2")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetDetails", b =>
                {
                    b.Property<int>("DivisionID")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear1")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear2")
                        .HasColumnType("int");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int");

                    b.Property<string>("GroupNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SubGroupNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LedgerNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<decimal>("ACAndBWPropRECurrFin")
                        .HasColumnType("money");

                    b.Property<decimal>("ACAndBWPropRENxtFin")
                        .HasColumnType("money");

                    b.Property<string>("ACBWJustificationBudgEstNxtFin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ACBWJustificationRevEst")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ActCurrFinTill2ndQuart")
                        .HasColumnType("money");

                    b.Property<decimal>("ActPrevFin")
                        .HasColumnType("money");

                    b.Property<decimal>("BudEstCurrFin")
                        .HasColumnType("money");

                    b.Property<decimal>("BudgEstNexFin")
                        .HasColumnType("money");

                    b.Property<decimal>("BudgEstNexFinProposed")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DelegateJustificationRevEst")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasAdminSaved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasDelegateSaved")
                        .HasColumnType("bit");

                    b.Property<decimal>("InterimRevEst")
                        .HasColumnType("money");

                    b.Property<string>("Justification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PerVarACBWRevEstOverBudgEstCurrFin")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("PerVarACBWRevEstOverBudgEstNxtFin")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("PerVarRevEstOverBudgEstCurrFin")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("PerVarRevEstOverBudgEstNxtFin")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("ProvisionalRevEst")
                        .HasColumnType("money");

                    b.Property<decimal>("RevEstCurrFin")
                        .HasColumnType("money");

                    b.Property<string>("SupportingDocumentPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber", "SubGroupNumber", "LedgerNumber");

                    b.ToTable("BudgetDetails");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetDetailsApproved", b =>
                {
                    b.Property<int>("DivisionID")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear1")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear2")
                        .HasColumnType("int");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int");

                    b.Property<string>("GroupNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SubGroupNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LedgerNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<decimal>("BudEstCurrFinACandBW")
                        .HasColumnType("money");

                    b.Property<decimal>("BudEstNextFin")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<decimal>("RevEstCurrFinACandBW")
                        .HasColumnType("money");

                    b.HasKey("DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber", "SubGroupNumber", "LedgerNumber");

                    b.ToTable("BudgetDetailsApproved");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetFiles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DivisionID")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinancialYear1")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear2")
                        .HasColumnType("int");

                    b.Property<string>("GroupNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LedgerNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int");

                    b.Property<string>("SubGroupNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SupportingDocumentPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.ToTable("BudgetFiles");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetGroups", b =>
                {
                    b.Property<string>("GroupNo")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("GroupName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SectionNo")
                        .HasColumnType("int");

                    b.HasKey("GroupNo");

                    b.HasIndex("SectionNo");

                    b.ToTable("BudgetGroups");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetLedgers", b =>
                {
                    b.Property<string>("LedgerNo")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("LedgerName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SubGroupNo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("LedgerNo");

                    b.HasIndex("SubGroupNo");

                    b.ToTable("BudgetLedgers");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetReports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ReportID")
                        .HasColumnType("int");

                    b.Property<string>("ReportName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BudgetReports");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetSections", b =>
                {
                    b.Property<int>("SectionNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("SectionName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("SectionNo");

                    b.ToTable("BudgetSections");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetSubGroups", b =>
                {
                    b.Property<string>("SubGroupNo")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("GroupNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("RequireInput")
                        .HasColumnType("bit");

                    b.Property<string>("subGroupName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("SubGroupNo");

                    b.HasIndex("GroupNo");

                    b.ToTable("BudgetSubGroups");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetdetailsStatus", b =>
                {
                    b.Property<int>("DivisionID")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear1")
                        .HasColumnType("int");

                    b.Property<int>("FinancialYear2")
                        .HasColumnType("int");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int");

                    b.Property<string>("GroupNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("ACBWSubmission")
                        .HasColumnType("bit");

                    b.Property<bool>("AdminEditStatus")
                        .HasColumnType("bit");

                    b.Property<bool>("ChairpersonApproval")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("DelegateEditStatus")
                        .HasColumnType("bit");

                    b.Property<bool>("FinCommitteeApproval")
                        .HasColumnType("bit");

                    b.Property<bool>("GenBodyApproval")
                        .HasColumnType("bit");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DivisionID", "FinancialYear1", "FinancialYear2", "SectionNumber", "GroupNumber");

                    b.ToTable("BudgetdetailsStatus", t =>
                        {
                            t.HasTrigger("trUpdateFinalSubmitStatus");
                        });
                });

            modelBuilder.Entity("BudgetPortal.Entities.Division", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DealingHandID")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("DivisionID")
                        .HasColumnType("int");

                    b.Property<string>("DivisionName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DivisionType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("State")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Division");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetGroups", b =>
                {
                    b.HasOne("BudgetPortal.Entities.BudgetSections", "Sections")
                        .WithMany("Groups")
                        .HasForeignKey("SectionNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sections");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetLedgers", b =>
                {
                    b.HasOne("BudgetPortal.Entities.BudgetSubGroups", "subGroups")
                        .WithMany("Ledgers")
                        .HasForeignKey("SubGroupNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("subGroups");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetSubGroups", b =>
                {
                    b.HasOne("BudgetPortal.Entities.BudgetGroups", "groups")
                        .WithMany("SubGroups")
                        .HasForeignKey("GroupNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("groups");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BudgetPortal.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BudgetPortal.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetPortal.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BudgetPortal.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetGroups", b =>
                {
                    b.Navigation("SubGroups");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetSections", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("BudgetPortal.Entities.BudgetSubGroups", b =>
                {
                    b.Navigation("Ledgers");
                });
#pragma warning restore 612, 618
        }
    }
}
