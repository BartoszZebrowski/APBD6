using Microsoft.EntityFrameworkCore;

namespace APBD6.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);
            modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);

            modelBuilder.Entity<PrescriptionMedicament>().HasKey(pm => new
            {
                pm.IdMedicament,
                pm.IdPrescription
            });

            modelBuilder.Entity<Patient>()
                .Property(p => p.IdPatient)
                .ValueGeneratedNever();

            modelBuilder.Entity<Doctor>()
                .Property(d => d.IdDoctor)
                .ValueGeneratedNever();

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(pm => pm.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdMedicament);

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Prescription)
                .WithMany(pm => pm.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdPrescription);
        }
    }
}
