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
        public DbSet<EnterpirseRequest> EnterpirseRequests {  get; set; }
        public DbSet<WorkerProfile> WorkerProfiles { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<WorkerShift> WorkerShifts { get; set; }
        public DbSet<AbsenceJustification> AbsenceJustifications { get; set; }
        public DbSet<JustificationDocument> JustificationDocuments { get; set; }
        public DbSet<OffdayVacationRequest> OffdayVacationRequests { get; set; }

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

            // Configuring EnterpirseRequest - User relationship
            modelBuilder.Entity<EnterpirseRequest>()
                .HasOne(er => er.User)  // Navigation property
                .WithMany(u => u.EnterpirseRequests)  // Navigation property on User
                .HasForeignKey(er => er.UserId);  // Foreign key in EnterpirseRequest

            // Define the relationship between Company and WorkerProfile
            modelBuilder.Entity<WorkerProfile>()
                .HasOne(w => w.Company)
                .WithMany(c => c.Workers)
                .HasForeignKey(w => w.CompanyId);

            // Define the relationship between Company and Building
            modelBuilder.Entity<Building>()
                .HasOne(b => b.Company)          // A building belongs to one company
                .WithMany(c => c.Buildings)      // A company has many buildings
                .HasForeignKey(b => b.CompanyId);

            // Define relationship between Movement and WorkerProfile
            modelBuilder.Entity<Movement>()
                .HasOne(m => m.Worker)
                .WithMany(w => w.Movements)
                .HasForeignKey(m => m.WorkerId);

            // One-to-many between Shift and Building
            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Building)
                .WithMany(b => b.Shifts)
                .HasForeignKey(s => s.BuildingId);

            // Many-to-many relationship between WorkerProfile and Shift via WorkerShift
            modelBuilder.Entity<WorkerShift>()
                .HasKey(ws => new { ws.WorkerId, ws.ShiftId });  // Composite key

            modelBuilder.Entity<WorkerShift>()
                .HasOne(ws => ws.Worker)
                .WithMany(w => w.WorkerShifts)
                .HasForeignKey(ws => ws.WorkerId);

            modelBuilder.Entity<WorkerShift>()
                .HasOne(ws => ws.Shift)
                .WithMany(s => s.WorkerShifts)
                .HasForeignKey(ws => ws.ShiftId);

            // One-to-many between AbsenceJustification and WorkerProfile
            modelBuilder.Entity<AbsenceJustification>()
                .HasOne(aj => aj.Worker)
                .WithMany(w => w.AbsenceJustifications)  // A worker can have multiple justifications
                .HasForeignKey(aj => aj.WorkerId);

            // One-to-many between AbsenceJustification and Company
            modelBuilder.Entity<AbsenceJustification>()
                .HasOne(aj => aj.Company)
                .WithMany(c => c.AbsenceJustifications)  // A company can have multiple justifications
                .HasForeignKey(aj => aj.CompanyId);

            // One-to-many between AbsenceJustification and JustificationDocument
            modelBuilder.Entity<JustificationDocument>()
                .HasOne(jd => jd.AbsenceJustification)
                .WithMany(aj => aj.JustificationDocuments)  // A justification can have multiple documents
                .HasForeignKey(jd => jd.AbsenceJustificationId);
            // One-to-many relationship between OffdayVacationRequest and WorkerProfile
            modelBuilder.Entity<OffdayVacationRequest>()
                .HasOne(ovr => ovr.Worker)
                .WithMany(w => w.OffdayVacationRequests)
                .HasForeignKey(ovr => ovr.WorkerId);

            // One-to-many relationship between OffdayVacationRequest and Company
            modelBuilder.Entity<OffdayVacationRequest>()
                .HasOne(ovr => ovr.Company)
                .WithMany(c => c.OffdayVacationRequests)
                .HasForeignKey(ovr => ovr.CompanyId);
            // One-to-one relationship between WorkerProfile and ApplicationUser
            modelBuilder.Entity<WorkerProfile>()
                .HasOne(wp => wp.ApplicationUser)
                .WithOne(au => au.WorkerProfile)
                .HasForeignKey<WorkerProfile>(wp => wp.ApplicationUserId); 
        }

    }
}
