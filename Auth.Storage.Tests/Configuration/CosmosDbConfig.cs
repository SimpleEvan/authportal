using Auth.Storage.Context;
using Auth.Storage.Tests.Configuration.Enum;
using Microsoft.EntityFrameworkCore;

namespace Auth.Storage.Tests.Configuration
{
    public static class CosmosDbConfig
    {
        public static DbContextOptions<AuthContext> GetOptionsAuthContext(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.CosmosDb)
            {
                return new DbContextOptionsBuilder<AuthContext>().UseCosmos(
                            "https://host",
                            "connectionString",
                            "databaseName").Options;
            }

            return new DbContextOptionsBuilder<AuthContext>().UseInMemoryDatabase("databaseName").Options;
        }

        public static void SeedDatabase()
        { 
        
        }
    }
}

