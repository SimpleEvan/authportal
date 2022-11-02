using Auth.Storage.Context;
using Auth.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Auth.Storage.Tests.Unittests.DbContext
{
    public class GetByIdCosmosDb
    {   
        private readonly AuthContext _sut;

        public GetByIdCosmosDb()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();
            _sut = new AuthContext(mockDbOptions.Object);
        }

        [Fact]
        public async Task GetById()
        {

            //Arrange
            var id = Guid.Parse("fd0adad4-a7b6-48ce-8745-43a5048c9765");
            await _sut.AuthTokens.AddRangeAsync(GetTokenData());

            // Act
            var token = await _sut.AuthTokens.FirstOrDefaultAsync();

            // Assert
            Assert.Equal(id, token?.id);
        }

        private List<AuthToken> GetTokenData()
        {
            return new List<AuthToken>
            {
                new AuthToken
                {
                    id = Guid.Parse("fd0adad4-a7b6-48ce-8745-43a5048c9765")
                }
            };
        }
    }
}

