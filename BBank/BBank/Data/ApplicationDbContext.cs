using BBank.Model;
using Microsoft.EntityFrameworkCore;

namespace BBank.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ClinicLocation> ClinicLocations { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClinicLocation>().ToTable("ClinicLocation");
            modelBuilder.Entity<User>().ToTable("User");
        }

    }
}
