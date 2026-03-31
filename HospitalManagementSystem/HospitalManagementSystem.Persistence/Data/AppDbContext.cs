using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace HospitalManagementSystem.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DIOR\MSSQLSERVER02;Initial Catalog=Hospital_Management_System_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Surname).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Diagnosis).HasMaxLength(500);
                entity.HasIndex(p => p.IsActive);
                entity.Ignore(p => p.FullName);
            });

            
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Surname).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
                entity.Property(d => d.RoomNumber).IsRequired();
                entity.Ignore(p => p.FullName);

            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Status).HasConversion<string>();

                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => a.PatientId);
                entity.HasIndex(a => a.DoctorId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Username).UseCollation("SQL_Latin1_General_CP1_CS_AS"); //Make case-sensetive
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Salt).IsRequired();
                entity.Property(u => u.Role).HasConversion<string>();

                entity.HasOne(u => u.Doctor)
                      .WithOne(d => d.User)
                      .HasForeignKey<User>(u => u.DoctorId)
                      .OnDelete(DeleteBehavior.SetNull);


            });


        }

    }

}
