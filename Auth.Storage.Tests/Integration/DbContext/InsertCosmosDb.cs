using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Enums;

namespace Auth.Storage.Tests.Integration.DbContext;

public class InsertCosmosDb
{
    [Fact]
    public async Task Insert()
    {
        var token = new AuthToken()
        {
            Duration = 3600,
            CreatedOn = DateTime.Now,
            id = Guid.NewGuid(),
            Resource = new Resource()
            {
                id = Guid.NewGuid(),
                Description = "Tests",
                Type = ResourceType.Application
            }
        };

        try
        {
            using (var context = new AuthContext(CosmosDbConfig.GetOptionsAuthContext()))
            {
                context.Add<AuthToken>(token);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        Assert.True(true);
    }
}
