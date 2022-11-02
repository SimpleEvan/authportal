using Auth.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Storage.Dependency
{
    public class CosmosDbSettings
    {
        public string Host { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }

    public static class CosmosDbServiceCollectionExtensions
    {
        private static DbContextOptions<AuthContext> AddConfig(CosmosDbSettings settings)
        {
            return new DbContextOptionsBuilder<AuthContext>().UseCosmos(
             settings.Host,
             settings.ConnectionString,
             settings.DatabaseName).Options;
        }

        public static IServiceCollection AddCosmosDbDependency(
             this IServiceCollection services, CosmosDbSettings settings)
        {
            services.AddSingleton<AuthContext>();

            services.AddSingleton<DbContext, AuthContext>((sp) =>
            {
                return new AuthContext(AddConfig(settings));
            });

            return services;
        }
    }
}

