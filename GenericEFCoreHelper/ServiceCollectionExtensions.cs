using GEFCH.IRepositories;
using GEFCH.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GenericEFCoreHelper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericEFCHelperRepository(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(IGenericRepository<,>), typeof(GenericRepository<,>), lifetime));
            return services;
        }
    }
}
