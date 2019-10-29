using IdentityServer4.EntityFramework.Options;
using JPProject.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using JPProject.Admin.Infra.Data.Context;

namespace JPProject.Admin.Infra.Data.Configuration
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddAdminContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, JpDatabaseOptions options = null)
        {
            if (options == null)
                options = new JpDatabaseOptions();

            var operationalStoreOptions = new OperationalStoreOptions();
            services.AddSingleton(operationalStoreOptions);

            var storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);

            services.AddDbContext<JpProjectContext>(optionsAction);
            services.AddDbContext<EventStoreContext>(optionsAction);

            DbMigrationHelpers.CheckDatabases(services.BuildServiceProvider(), options).Wait();

            return services;
        }
    }
}
