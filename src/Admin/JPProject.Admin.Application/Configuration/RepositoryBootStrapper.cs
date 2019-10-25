using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Interfaces;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Admin.Infra.Data.EventSourcing;
using JPProject.Admin.Infra.Data.Repository;
using JPProject.Admin.Infra.Data.Repository.EventSourcing;
using JPProject.Admin.Infra.Data.UoW;
using JPProject.EntityFrameworkCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Admin.Application.Configuration
{
    internal class RepositoryBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IPersistedGrantRepository, PersistedGrantRepository>();
            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
            services.AddScoped<IApiScopeRepository, ApiScopeRepository>();

            services.AddScoped<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientSecretRepository, ClientSecretRepository>();
            services.AddScoped<IApiSecretRepository, ApiSecretRepository>();

            services.AddScoped<IClientClaimRepository, ClientClaimRepository>();
            services.AddScoped<IClientPropertyRepository, ClientPropertyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IdentityServerContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreContext>();
        }
    }
}
