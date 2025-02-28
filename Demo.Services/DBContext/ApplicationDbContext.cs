using Demo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Parameterless constructor for design-time support
        public ApplicationDbContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define your model configurations here
        }

        // Define DbSets for your entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
