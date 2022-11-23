using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Tests.Configuration;
using Auth.Storage.Tests.Configuration.Enum;

namespace Auth.Storage.Tests.Integration.DbContext
{
    public class GetByIdCosmosDb: IDisposable
    {
        private AuthContext _context;
        public GetByIdCosmosDb()
        {
            _context = new AuthContext(CosmosDbConfig.GetOptionsAuthContext(DatabaseType.InMemoryDb));
            _context.Database.EnsureCreated();
            CosmosDbConfig.SeedDatabase();
        }

        [Fact]
        public void GetById()
        {
            var id = Guid.Parse("d3aca852-8669-4d38-97d7-fa267e9d2574");
            var token = new AuthToken();


        }

        public void Dispose() 
        {
            _context.Database.EnsureDeleted();
        }
    }
}

