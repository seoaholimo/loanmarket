using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.BeyontDebtors;
using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Domain.Loans;
using BeyondIT.MicroLoan.Domain.Navigation;
using BeyondIT.MicroLoan.Domain.Page;
using BeyondIT.MicroLoan.Domain.Role;
using BeyondIT.MicroLoan.Domain.Users;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BeyondIT.MicroLoan.Infrastructure.Database
{
    public partial class MicroLoanContext : DbContext
    {
        private readonly string _connectionString;

        public MicroLoanContext(IAppSettingsAccessor appSettingsAccessor)
        {
            _connectionString = appSettingsAccessor.AppSettings.Data.DefaultConnection.ConnectionString;
        }
        public MicroLoanContext()
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .AddJsonFile("appsettings.Development.json", true);

            IConfigurationRoot configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("MicroLoanDb");
        }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<ClientPageRoles> ClientPageRoles { get; set; }
        public virtual DbSet<ClientUserRoles> ClientUserRoles { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Debtor> Debtor { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LoanLedger> LoanLedger { get; set; }
        public virtual DbSet<LoanProcess> LoanProcess { get; set; }
        public virtual DbSet<LoanSettings> LoanSettings { get; set; }
        public virtual DbSet<LoanStage> LoanStage { get; set; }
        public virtual DbSet<Navigations> Navigations { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseSqlServer(_connectionString, builder => builder.CommandTimeout(180))
               .ConfigureWarnings(builder => builder.Ignore(CoreEventId.DetachedLazyLoadingWarning));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Address1)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CellNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TelNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Village)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClientPageRoles>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.PageId, e.RoleId, e.ModuleId });

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPageRoles)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.ClientPageRolesCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_ClientPageRoles_Users_CreatedById");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.ClientPageRoles)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ClientPageRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UpdatedBy)
                    .WithMany(p => p.ClientPageRolesUpdatedBy)
                    .HasForeignKey(d => d.UpdatedById)
                    .HasConstraintName("FK_ClientPageRoles_Users_UpdatedById");
            });

            modelBuilder.Entity<ClientUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.UserId, e.RoleId });

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientUserRoles)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.ClientUserRolesCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_ClientUserRoles_Users_CreatedById");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ClientUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UpdatedBy)
                    .WithMany(p => p.ClientUserRolesUpdatedBy)
                    .HasForeignKey(d => d.UpdatedById)
                    .HasConstraintName("FK_ClientUserRoles_Users_UpdatedById");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClientUserRolesUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientUserRoles_Users_UserId");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.GlCode).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhysicalAddressCity)
                    .HasColumnName("PhysicalAddress_City")
                    .HasMaxLength(50);

                entity.Property(e => e.PhysicalAddressPostalCode)
                    .HasColumnName("PhysicalAddress_PostalCode")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalAddress).HasMaxLength(50);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.ClientsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_Clients_Users_CreatedById");

                entity.HasOne(d => d.ParentClient)
                    .WithMany(p => p.InverseParentClient)
                    .HasForeignKey(d => d.ParentClientId);

                entity.HasOne(d => d.UpdatedBy)
                    .WithMany(p => p.ClientsUpdatedBy)
                    .HasForeignKey(d => d.UpdatedById)
                    .HasConstraintName("FK_Clients_Users_UpdatedById");
            });

            modelBuilder.Entity<Debtor>(entity =>
            {
                entity.Property(e => e.DebtorId).HasColumnName("Debtor_Id");

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdentificationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Debtor)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK__Debtor__AddressI__2A164134");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Debtor)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__Debtor__ClientId__2B0A656D");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.LoanId).ValueGeneratedNever();

                entity.Property(e => e.AmountApproved)
                    .HasColumnName("Amount_Approved")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AmountRequest)
                    .HasColumnName("Amount_Request")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.DebtorId).HasColumnName("Debtor_Id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LoanStatus)
                    .HasColumnName("Loan_Status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");

                entity.Property(e => e.PaymentFrequency).HasColumnName("payment_frequency");

                entity.Property(e => e.StageId).HasColumnName("stageId");
                
               });

            modelBuilder.Entity<LoanLedger>(entity =>
            {
                entity.ToTable("Loan_Ledger");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateRecorded)
                    .HasColumnName("dateRecorded")
                    .HasColumnType("datetime");

                entity.Property(e => e.DebtorId).HasColumnName("Debtor_Id");

                entity.Property(e => e.LoanId).HasColumnName("Loan_Id");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnName("payment_amount")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.LoanLedger)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Loan_Ledg__Creat__76969D2E");

                entity.HasOne(d => d.Debtor)
                    .WithMany(p => p.LoanLedger)
                    .HasForeignKey(d => d.DebtorId)
                    .HasConstraintName("FK__Loan_Ledg__Debto__778AC167");

                entity.HasOne(d => d.Loan)
                    .WithMany(p => p.LoanLedger)
                    .HasForeignKey(d => d.LoanId)
                    .HasConstraintName("FK__Loan_Ledg__Loan___787EE5A0");
            });

            modelBuilder.Entity<LoanProcess>(entity =>
            {
                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.LoanId).HasColumnName("Loan_Id");

                entity.Property(e => e.StageId).HasColumnName("Stage_Id");

                entity.HasOne(d => d.Loan)
                    .WithMany(p => p.LoanProcess)
                    .HasForeignKey(d => d.LoanId)
                    .HasConstraintName("FK__LoanProce__Loan___66603565");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.LoanProcess)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK__LoanProce__Stage__656C112C");
            });

            modelBuilder.Entity<LoanSettings>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<LoanStage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");

                entity.Property(e => e.StageDescription)
                    .HasColumnName("Stage_Description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StageStatus)
                    .HasColumnName("Stage_Status")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Navigations>(entity =>
            {
                entity.HasKey(e => e.NavigationId);

                entity.Property(e => e.NavigationId).ValueGeneratedNever();

                entity.Property(e => e.QueryString).HasMaxLength(512);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.Navigations)
                    .HasForeignKey(d => d.PageId);

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.ParentNavigationId);
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.ClientId);

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.RolesCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_Roles_Users_CreatedById");

                entity.HasOne(d => d.ParentRole)
                    .WithMany(p => p.InverseParentRole)
                    .HasForeignKey(d => d.ParentRoleId);

                entity.HasOne(d => d.UpdatedBy)
                    .WithMany(p => p.RolesUpdatedBy)
                    .HasForeignKey(d => d.UpdatedById)
                    .HasConstraintName("FK_Roles_Users_UpdatedById");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CellphoneNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityDocumentNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResetPasswordToken).IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TokenExpiryDate).HasColumnType("datetime");
            });
        }
        public void Save()
        {
            SaveChanges();
        }

        public T Refresh<T>(DbSet<T> collection, Expression<Func<T, bool>> predicate) where T : class
        {
            T entity = collection.First(predicate);
            Entry(entity).State = EntityState.Detached;
            entity = collection.First(predicate);
            return entity;
        }

        public void ExecuteSqlCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlCommand(query, parameters);
        }

        public IEnumerable<T> SqlQuery<T>(string query) where T : class
        {
            return Query<T>().FromSql(query);
        }

        public IEnumerable<T> SqlQuery<T>(string query, params object[] parameters) where T : class
        {
            return Query<T>().FromSql(query, parameters);
        }
    }
}
