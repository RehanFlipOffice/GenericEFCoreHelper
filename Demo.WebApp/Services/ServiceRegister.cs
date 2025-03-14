﻿using Demo.Services.DBContext;
using Demo.Services.IServices;
using Demo.Services.Services;
using GenericEFCoreHelper;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Services
{
    public static class ServiceRegister
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Entity Framework Core
            var connectionString =
                configuration["ConnectionStrings:DemoWebAppConnectionString"]
                ?? throw new InvalidOperationException("Connection string 'DemoWebAppConnectionString' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

            // Register Repositories
            // Uses default ServiceLifetime.Scoped
            services.AddGenericEFCHelperRepository();
            // or Specify ServiceLifetime.Singleton
            //services.AddGenericEFCHelperRepository(ServiceLifetime.Singleton); 

            services.AddTransient<IEmployeeService, EmployeeService>();
        }
    }
}
