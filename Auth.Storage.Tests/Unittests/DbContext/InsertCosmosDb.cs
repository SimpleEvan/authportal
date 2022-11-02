using Auth.Storage.Context;
using Auth.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Auth.Storage.Tests.Unittests.DbContext
{
    public class InsertCosmosDb
    {
        private readonly AuthContext _sut;

        public InsertCosmosDb()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();
            _sut = new AuthContext(mockDbOptions.Object);
        }

        [Fact]
        public async Task GetById()
        {

            //Arrange
            await _sut.AuthTokens.AddRangeAsync(GetTokenData());

            // Act
            var tokens = await _sut.AuthTokens.ToListAsync();

            // Assert
            Assert.Equal(2, tokens.Count());
        }

        private List<AuthToken> GetTokenData()
        {
            return new List<AuthToken>
            {
                new AuthToken
                {
                    id = Guid.Empty
                },
                new AuthToken
                {
                    id = Guid.Empty
                }
            };
        }
    }
}

