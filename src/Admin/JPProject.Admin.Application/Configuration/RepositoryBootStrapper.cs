using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Admin.Application.Configuration
{
    internal static class RepositoryBootStrapper
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {

            // Infra - Data EventSourcing

            return services;
        }
    }
}
