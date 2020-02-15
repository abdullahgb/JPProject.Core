using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Admin.Infra.Data.Interfaces;
using JPProject.Admin.Infra.Data.UoW;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.EventSourcing;
using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using JPProject.Admin.Infra.Data.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextConfiguration
    {
        public static IJpProjectAdminBuilder AddAdminContext(this IJpProjectAdminBuilder services, Action<DbContextOptionsBuilder> optionsAction, JpDatabaseOptions options = null)
        {
            if (options == null)
                options = new JpDatabaseOptions();

            RegisterDatabaseServices(services.Services);


            var operationalStoreOptions = new OperationalStoreOptions();
            services.Services.AddSingleton(operationalStoreOptions);

            var storeOptions = new ConfigurationStoreOptions();
            services.Services.AddSingleton(storeOptions);

            services.Services.AddDbContext<JPProjectAdminUIContext>(optionsAction);

            //DbMigrationHelpers.CheckDatabases(services.BuildServiceProvider(), options).Wait();

            return services;
        }

        private static void RegisterDatabaseServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IAdminContext>(x => x.GetService<JPProjectAdminUIContext>());
            services.AddScoped<IConfigurationDbContext>(x => x.GetService<JPProjectAdminUIContext>());
            services.AddScoped<IConfigurationStore>(x => x.GetService<JPProjectAdminUIContext>());
            services.AddScoped<IEventStore, SqlEventStore>();
        }

        public static IJpProjectAdminBuilder AddEventStore<TEventStore>(this IJpProjectAdminBuilder services)
            where TEventStore : class, IEventStoreContext
        {
            services.Services.AddScoped<IEventStoreContext, TEventStore>();
            return services;
        }
    }
}
