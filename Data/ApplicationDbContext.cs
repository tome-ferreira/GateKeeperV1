using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Controllers;
using GateKeeperV1.Models;

namespace GateKeeperV1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAdmins> CompanyAdmins { get; set; }
        public DbSet<CompanySupervisors> CompanySupervisors { get; set; }
        public DbSet<CompanyManagers> CompanyManagers { get; set; }
        public DbSet<CompanyWorkers> ComapnyWorkers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Plan>()
                .Property(p => p.MonthlyPrice)
                .HasPrecision(18, 2); // Precision 18, Scale 2

            modelBuilder.Entity<Plan>()
                .Property(p => p.AnualPrice)
                .HasPrecision(18, 2); // Precision 18, Scale 2



            //Project role Admin (Many to many)
            modelBuilder.Entity<CompanyAdmins>()
               .HasKey(pu => new { pu.UserId, pu.CompanyId });

            modelBuilder.Entity<CompanyAdmins>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.CompanyAdmins)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<CompanyAdmins>()
                .HasOne(pu => pu.Company)
                .WithMany(p => p.CompanyAdmins)
                .HasForeignKey(pu => pu.CompanyId);



            //Project role Manager (Many to many)
            modelBuilder.Entity<CompanyManagers>()
               .HasKey(pu => new { pu.UserId, pu.CompanyId });

            modelBuilder.Entity<CompanyManagers>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.CompanyManagers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<CompanyManagers>()
                .HasOne(pu => pu.Company)
                .WithMany(p => p.CompanyManagers)
                .HasForeignKey(pu => pu.CompanyId);


            //Project role Supervisor (Many to many)
            modelBuilder.Entity<CompanySupervisors>()
                .HasKey(pu => new { pu.UserId, pu.CompanyId });

            modelBuilder.Entity<CompanySupervisors>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.CompanySupervisors)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<CompanySupervisors>()
                .HasOne(pu => pu.Company)
                .WithMany(p => p.CompanySupervisors)
                .HasForeignKey(pu => pu.CompanyId);


            //Project role Worker (Many to many)
            modelBuilder.Entity<CompanyWorkers>()
                .HasKey(pu => new { pu.UserId, pu.CompanyId });

            modelBuilder.Entity<CompanyWorkers>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ComapanyWorkers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<CompanyWorkers>()
                .HasOne(pu => pu.Company)
                .WithMany(p => p.CompanyWorkers)
                .HasForeignKey(pu => pu.CompanyId);



            //Company plan (One to many)
            modelBuilder.Entity<Plan>()
                .HasMany(c => c.Companies)
                .WithOne(p => p.Plan)
                .HasForeignKey(p => p.PlanId);
        }
    }
}
