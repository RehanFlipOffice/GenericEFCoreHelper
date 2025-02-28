using GEFCH.IRepositories;
using GEFCH.Repositories;
using GenericEFCoreHelper.Tests.DBContext;
using GenericEFCoreHelper.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericEFCoreHelper.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        private ServiceProvider BuildServiceProvider(ServiceLifetime lifetime)
        {
            var services = new ServiceCollection();

            // Register the ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"), lifetime);

            // Register the GenericEFCHelperRepository
            services.AddGenericEFCHelperRepository(lifetime);

            return services.BuildServiceProvider();
        }

        [Fact]
        public void AddGenericEFCHelperRepository_RegistersScopedServiceByDefault()
        {
            // Arrange
            var serviceProvider = BuildServiceProvider(ServiceLifetime.Scoped);

            // Act
            var repository = serviceProvider.GetService<IGenericRepository<Employee, ApplicationDbContext>>();

            // Assert
            Assert.NotNull(repository);
            Assert.IsType<GenericRepository<Employee, ApplicationDbContext>>(repository);
        }

        [Fact]
        public void AddGenericEFCHelperRepository_RegistersSingletonService()
        {
            // Arrange
            var serviceProvider = BuildServiceProvider(ServiceLifetime.Singleton);

            // Act
            var repository1 = serviceProvider.GetService<IGenericRepository<Employee, ApplicationDbContext>>();
            var repository2 = serviceProvider.GetService<IGenericRepository<Employee, ApplicationDbContext>>();

            // Assert
            Assert.NotNull(repository1);
            Assert.NotNull(repository2);
            Assert.Same(repository1, repository2);
        }

        [Fact]
        public void AddGenericEFCHelperRepository_RegistersTransientService()
        {
            // Arrange
            var serviceProvider = BuildServiceProvider(ServiceLifetime.Transient);

            // Act
            var repository1 = serviceProvider.GetService<IGenericRepository<Employee, ApplicationDbContext>>();
            var repository2 = serviceProvider.GetService<IGenericRepository<Employee, ApplicationDbContext>>();

            // Assert
            Assert.NotNull(repository1);
            Assert.NotNull(repository2);
            Assert.NotSame(repository1, repository2);
        }
    }
}
