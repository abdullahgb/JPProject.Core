using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.Repository;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.EntityFramework.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.EntityFramework.Repository.Configuration
{
    internal static class RepositoryBootStrapper
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IGlobalConfigurationSettingsRepository, GlobalConfigurationSettingsRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();

            return services;
        }
    }
}
