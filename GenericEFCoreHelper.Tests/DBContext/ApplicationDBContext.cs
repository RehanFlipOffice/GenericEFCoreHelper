using GenericEFCoreHelper.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericEFCoreHelper.Tests.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add custom configurations for your entities here
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Additional configurations
            });
        }

        // Define DbSets for your entities
        public DbSet<Employee> Employees { get; set; }
    }
}
