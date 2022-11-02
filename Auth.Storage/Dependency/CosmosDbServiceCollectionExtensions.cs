using Auth.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Storage.Dependency
{
    public static class CosmosDbServiceCollectionExtensions
    {
        private static DbContextOptions<AuthContext> AddConfig(IConfiguration config)
        {
            // Get from settings var db = config.GetSection("database");

            return new DbContextOptionsBuilder<AuthContext>().UseCosmos(
             "https://host/",
             "ConnectionString",
             "DbName").Options;
        }

        public static IServiceCollection AddCosmosDbDependency(
             this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<AuthContext>();

            services.AddScoped<DbContext, AuthContext>((sp) =>
            {
                return new AuthContext(AddConfig(config));
            });

            return services;
        }
    }
}

