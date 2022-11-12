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

namespace JwtAuth.API.Tests.Services.AuthService
{
    public class LoginTests
    {
        private readonly Mock<AuthContext> _context;
        private readonly Mock<IOptions<AuthPortalServiceOptions>> _options;
        private readonly IAuthPortalService _authPortalService;

        public LoginTests()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();
      
            _context = new Mock<AuthContext>();
            _options = new Mock<IOptions<AuthPortalServiceOptions>>();

            var _tokensSet = AuthTokenExampleData.GetTokenData().BuildMockDbSet();

            _context.Setup(t => t.AuthTokens).Returns(_tokensSet.Object);
            _options.Setup(o => o.Value).Returns(new AuthPortalServiceOptions { IssuerSecretKey = "top secret" });

            _authPortalService = new AuthPortalService(_context.Object, _options.Object);
        }

        [Fact]
        public async Task WhenCallingLoginWithIncorrectPasswordShouldReturnEmptyTokens()
        {
            // Arrange
            var userName = "username";
            var password = "password";
            
            // Act
            var sut = await _authPortalService.Login(new UserRequest { Username = userName, Password = password });

            // Assert
            Assert.Equal(string.Empty, sut.JwtToken);
            Assert.Equal(string.Empty, sut.RefreshToken);
        }
    }
}

