using Clinic_system.Helpers;
using Clinic_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_system.Data
{
    public class ClinicdbContext: DbContext
    {
        public ClinicdbContext(DbContextOptions contextOptions):base(contextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role { RoleId = 1, RoleName = "Admin" },
                    new Role { RoleId = 2, RoleName = "Doctor" },
                    new Role { RoleId = 3, RoleName = "Receptionist" });


            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, RoleId = 1, FullName = "Admin", Password = PasswordHelper.HashPassword("100200300"), Email = "admin@admin.com" });

            base.OnModelCreating(modelBuilder);
        }


    }
}
