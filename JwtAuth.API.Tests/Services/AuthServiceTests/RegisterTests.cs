using Auth.Storage.Context;
using Auth.Storage.Entities;
using JwtAuth.API.APIModels;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using JwtAuth.API.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;

namespace JwtAuth.API.Tests.Services.AuthServiceTests
{
    public class RegisterTests
    {
        private readonly IAuthPortalService _sut;
        private readonly Mock<AuthContext> _context;
        private readonly Mock<DbSet<AuthToken>> _tokensSet;
        private readonly Mock<IOptions<AuthPortalServiceOptions>> _options;

        public RegisterTests()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();
            _options = new Mock<IOptions<AuthPortalServiceOptions>>();

            _tokensSet = AuthTokenExampleData.GetTokenData().BuildMockDbSet();

            _context = new Mock<AuthContext>();
            _context.Setup(o => o.AuthTokens).Returns(_tokensSet.Object);
            _options.Setup(o => o.Value).Returns(new AuthPortalServiceOptions { IssuerSecretKey = "top secret" });

            _sut = new AuthPortalService(_context.Object, _options.Object);
        }

        [Fact]
        public async Task WhenCallingRegisterShouldReturnAuthTokenResponse()
        {
            // Arrange
            var userName = "username";
            var password = "password";
            var accessToken = "accesToken";

            // Act
            var sut = await _sut.Register(new UserRequest
            { 
                Username = userName,
                Password = password,
            }, accessToken);

            // Assert
            Assert.Equal(string.Empty, sut.UserName);
        }

        //[Fact]
        //public async Task WhenCallingRegisterShouldReturnAuthTokenResponse()
        //{
        //    _tokensSet.Verify(m => m.Add(It.IsAny<AuthToken>()), Times.Once());
        //    _context.Verify(m => m.SaveChanges(), Times.Once());
        //}
    }
}

