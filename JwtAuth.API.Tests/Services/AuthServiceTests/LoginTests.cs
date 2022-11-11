using System.Collections.Generic;
using System.Reflection.Metadata;
using Auth.Storage.Context;
using Auth.Storage.Entities;
using JwtAuth.API.APIModels;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using JwtAuth.API.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JwtAuth.API.Tests.Services.AuthService
{
    public class LoginTests
    {
        private readonly Mock<AuthContext> _context;
        private readonly Mock<DbSet<AuthToken>> _tokensSet;
        private readonly Mock<IOptions<AuthPortalServiceOptions>> _options;
        private readonly IAuthPortalService _authPortalService;

        public LoginTests()
        {
            var mockDbOptions = new Mock<DbContextOptions<AuthContext>>();

            _tokensSet = new Mock<DbSet<AuthToken>>();
            _context = new Mock<AuthContext>();
            _options = new Mock<IOptions<AuthPortalServiceOptions>>();

            //_tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.Provider).Returns(AuthTokenExampleData.GetTokenData().Provider);
            //_tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.<AuthToken>()).Returns(AuthTokenExampleData.GetTokenData().Expression);
            //_tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.Expression).Returns(AuthTokenExampleData.GetTokenData().Expression);
            //_tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.ElementType).Returns(AuthTokenExampleData.GetTokenData().ElementType);
            //_tokensSet.As<IDbAsyncEnumerable<AuthToken>>().Setup(m => m.GetAsyncEnumerator()).Returns(() => AuthTokenExampleData.GetTokenData().GetEnumerator());



            //_tokensSet.As<IDbAsyncEnumerable<AuthToken>>()
            //.Setup(m => m.GetAsyncEnumerator())
            //    .Returns(new TestDbAsyncEnumerator<AuthToken>(AuthTokenExampleData.GetTokenData().GetEnumerator()));
            //_tokensSet.As<IQueryable<AuthToken>>()
            //.Setup(m => m.Provider)
            //.Returns(new TestDbAsyncQueryProvider<AuthToken>(AuthTokenExampleData.GetTokenData().Provider));

            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.Expression).Returns(AuthTokenExampleData.GetTokenData().Expression);
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.ElementType).Returns(AuthTokenExampleData.GetTokenData().ElementType);
            _tokensSet.As<IQueryable<AuthToken>>().Setup(m => m.GetEnumerator()).Returns(() => AuthTokenExampleData.GetTokenData().GetEnumerator());


            _context.Setup(t => t.AuthTokens).Returns(_tokensSet.Object);
            _options.Setup(o => o.Value).Returns(new AuthPortalServiceOptions { IssuerSecretKey = "top secret" });

            _authPortalService = new AuthPortalService(_context.Object, _options.Object);
        }

        [Fact]
        public async Task WhenCallingLoginShouldReturnSuccessfulLogin()
        {
            try
            {
                // Arrange
                
                // Act
                var sut = await _authPortalService.Login(new UserRequest { Username = "" , Password =""});

                // Assert
                //_tokensSet.Verify(m => m.SingleOrDefault(el => el.Username == It.IsAny<string>()), Times.Once());
                _context.Verify(m => m.SaveChanges(), Times.Never());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

