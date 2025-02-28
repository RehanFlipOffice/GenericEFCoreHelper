using Demo.Services.DBContext;
using Microsoft.EntityFrameworkCore;
using GenericEFCoreHelper;
using Demo.Services.IServices;
using Demo.Services.Services;

namespace Demo.API.Extensions
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Entity Framework Core
            var connectionString =
                configuration["ConnectionStrings:DemoAPIDBConnecction"]
                ?? throw new InvalidOperationException("Connection string 'DemoAPIDBConnecction' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

            // Register Repositories
            services.AddGenericEFCHelperRepository();

            services.AddTransient<IEmployeeService, EmployeeService>();
        }
    }
}
