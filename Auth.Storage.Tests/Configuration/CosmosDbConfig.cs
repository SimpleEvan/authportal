using Auth.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Storage.Tests.Configuration
{
    public static class CosmosDbConfig
    {
        public static DbContextOptions<AuthContext> GetOptionsAuthContext()
        {
            return new DbContextOptionsBuilder<AuthContext>().UseCosmos(
             "https://host",
             "ConnectionString",
             "DbName").Options;
        }
    }
}

