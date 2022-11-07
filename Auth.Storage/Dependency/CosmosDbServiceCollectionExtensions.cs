
using Auth.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddCosmosDbDependency(
             this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AuthContext>(
                options => options.UseCosmos(
                     configuration.GetSection("databaseHost").Value,
                     configuration.GetSection("CosmosDbSettingsConnectionString").Value,
                     configuration.GetSection("databaseName").Value));

            return services;
        }
    }
}

