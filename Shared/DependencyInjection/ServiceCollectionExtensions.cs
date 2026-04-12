using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Shared.Configuration;
using EventScheduler.Logging;
using EventScheduler.Repositories;
using EventScheduler.Services;
using EventScheduler.Interfaces;
using LinqQuery.Interfaces;
using LinqQuery.Services;

namespace Shared.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        // Registers application services into the DI container
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind configuration to strongly typed options
            services.Configure<StorageOptions>(
                configuration.GetSection("Storage"));

            // Logging
            services.AddSingleton<ILogger, EventLogger>();

            // Repositories & Services
            services.AddSingleton<IEventRepository, FileEventRepository>();

            // Business Services
            services.AddScoped<IEventService, EventService>();
            services.AddSingleton<INumberService, NumberService>();

            return services;
        }
    }
}