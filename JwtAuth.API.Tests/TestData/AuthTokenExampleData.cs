using Auth.Storage.Entities;
namespace JwtAuth.API.Tests.TestData
{
    public class AuthTokenExampleData
	{
        public static IQueryable<AuthToken> GetTokenData()
        {
            return new List<AuthToken>
            {
                new AuthToken
                {
                    id = Guid.Parse("fd0adad4-a7b6-48ce-8745-43a5048c9765")
                }
            }.AsQueryable();
        }
    }
}