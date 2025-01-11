using Ams.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ams.Api.Context
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(a => a.Id)
                .HasConversion(
                    v => v.ToString(),
                    v => Ulid.Parse(v)
                );
        }
    }

}
