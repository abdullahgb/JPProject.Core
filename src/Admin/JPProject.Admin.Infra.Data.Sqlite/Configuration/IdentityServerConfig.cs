using JPProject.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using JPProject.Admin.Infra.Data.Configuration;
using JPProject.Admin.Infra.Data.Context;

namespace JPProject.Admin.Infra.Data.Sqlite.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection WithSqlite(this IServiceCollection services, string connectionString, JpDatabaseOptions options = null)
        {
            var migrationsAssembly = typeof(DatabaseConfig).GetTypeInfo().Assembly.GetName().Name;
            services.AddEntityFrameworkSqlite().AddAdminContext(opt => opt.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), options);

            return services;
        }

    }
}