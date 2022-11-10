using System;
using System.Net.Sockets;
using Auth.Storage.Context;
using Auth.Storage.Entities;
using JwtAuth.API.APIModels;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

            _tokensSet = new Mock<DbSet<AuthToken>>();
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.Provider).Returns(GetTokenData().Provider);
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.Expression).Returns(GetTokenData().Expression);
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.ElementType).Returns(GetTokenData().ElementType);
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.GetEnumerator()).Returns(() => GetTokenData().GetEnumerator());

            _context = new Mock<AuthContext>();
            _context.Object.AddRangeAsync(_tokensSet.Object);
            _options.Setup(o => o.Value).Returns(new AuthPortalServiceOptions { IssuerSecretKey = "top secret" });

            _sut = new AuthPortalService(_context.Object, _options.Object);
        }

        [Fact]
        public async Task WhenCallingRegisterShouldReturnAuthTokenResponse()
        {
            // Arrange


            // Act
            var sut = await _sut.Register(It.IsAny<UserRequest>(), It.IsAny<string>());

            // Assert
            _tokensSet.Verify(m => m.Add(It.IsAny<AuthToken>()), Times.Once());
            _context.Verify(m => m.SaveChanges(), Times.Once());
        }

        private IQueryable<AuthToken> GetTokenData()
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

