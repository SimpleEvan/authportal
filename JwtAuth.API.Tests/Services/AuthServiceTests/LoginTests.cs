using Auth.Storage.Context;
using Auth.Storage.Entities;
using JwtAuth.API.APIModels;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace JwtAuth.API.Tests.Services.AuthService
{
    public class LoginTests
    {
        private readonly IAuthPortalService _sut;
        private readonly Mock<AuthContext> _context;
        private readonly Mock<DbSet<AuthToken>> _tokensSet;
        private readonly Mock<IOptions<AuthPortalServiceOptions>> _options;

        public LoginTests()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();

            _tokensSet = new Mock<DbSet<AuthToken>>();
            _context = new Mock<AuthContext>();
            _options = new Mock<IOptions<AuthPortalServiceOptions>>();

            _context.Setup(t => t.AuthTokens).Returns(_tokensSet.Object);
            _options.Setup(o => o.Value).Returns(new AuthPortalServiceOptions { IssuerSecretKey = "top secret" });

            _sut = new AuthPortalService(_context.Object, _options.Object);
        }

        [Fact]
        public async Task WhenCallingLoginShouldReturnSuccessfulLogin()
        {
            // Arrange


            // Act
            var sut = await _sut.Login(It.IsAny<UserRequest>());

            // Assert
            _tokensSet.Verify(m => m.SingleOrDefault(el => el.Username == It.IsAny<string>()), Times.Once());
            _context.Verify(m => m.SaveChanges(), Times.Never());
        }
    }
}

