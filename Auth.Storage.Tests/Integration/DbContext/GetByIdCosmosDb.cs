using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Tests.Configuration;

namespace Auth.Storage.Tests.Integration.DbContext
{
    public class GetByIdCosmosDb
    {
        public GetByIdCosmosDb()
        {
            //var dbName = $"AuthorPostsDb_{DateTime.Now.ToFileTimeUtc()}";
            //dbContextOptions = new DbContextOptionsBuilder<BlogDbContext>()
            //    .UseInMemoryDatabase(dbName)
            //    .Options;
        }

        [Fact]
        public void GetById()
        {
            var id = Guid.Parse("d3aca852-8669-4d38-97d7-fa267e9d2574");
            var token = new AuthToken();

            try
            {
                using (var context = new AuthContext(CosmosDbConfig.GetOptionsAuthContext()))
                {
                    if (context.AuthTokens != null)
                    {
                        token = context.AuthTokens.SingleOrDefault(element => element.id == id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Assert.Equal(token?.id, id);
        }
    }
}

