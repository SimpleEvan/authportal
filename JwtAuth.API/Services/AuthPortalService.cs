using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Enums;
using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.API.Services
{
    public class AuthPortalService : IAuthPortalService
    {
        private readonly AuthContext _context;
        private readonly AuthPortalServiceOptions _options;

        public AuthPortalService(AuthContext context, IOptions<AuthPortalServiceOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        public async Task<AuthTokenResponse> Register(UserRequest userRequest, string accessToken)
        {
            try
            {
                var portalOwner = await _context.AuthTokens.SingleOrDefaultAsync(element => element.Username.ToLower() == "chell");

                if (portalOwner == null || !portalOwner.Hash.Equals(accessToken))
                {
                    return new AuthTokenResponse();
                }

                var hashObj = HashGenerator.CreateHash(userRequest.Password);

                await _context.AddAsync(new AuthToken
                {
                    Username = userRequest.Username,
                    Duration = 3600,
                    Hash = Convert.ToBase64String(hashObj.hash),
                    Salt = Convert.ToBase64String(hashObj.salt),
                    Resource = new Resource
                    {
                        Description = "Register auth token",
                        Type = ResourceType.Tenant
                    },
                    RefreshToken = new RefreshToken
                    {
                        Token = HashGenerator.GeneratedRefreshToken(),
                        ExpiresOn = DateTime.Now.AddDays(7)

                    }
                });

                await _context.SaveChangesAsync();

                return new AuthTokenResponse
                {
                    UserName = userRequest.Username,
                    ExpiresAt = DateTime.Now.AddSeconds(3600),
                    Duration = 3600
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuthLoginResponse> Login(UserRequest userRequest)
        {
            try
            {
                var user = await _context.AuthTokens.SingleOrDefaultAsync(element => element.Username == userRequest.Username);

                if (user != null)
                {
                    if (!HashGenerator.VerifyPasswordHash(userRequest.Password, Convert.FromBase64String(user.Hash), Convert.FromBase64String(user.Salt)))
                    {
                        return new AuthLoginResponse();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userRequest.Username),
                        new Claim(ClaimTypes.Role, nameof(AuthorizationRoles.Dolphin))
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.IssuerSecretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                    return new AuthLoginResponse
                    {
                        JwtToken = jwt,
                        RefreshToken = user.RefreshToken.Token,
                        RefreshTokenExpiresOn = user.RefreshToken.ExpiresOn
                    };
                }
                else
                {
                    return new AuthLoginResponse();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AutRefreshTokenResponse> RecycleRefreshToken(string userName, string refreshToken)
        {
            try
            {
                var user = await _context.AuthTokens.SingleOrDefaultAsync(element => element.Username == userName);

                if (user == null || !user.RefreshToken.Equals(refreshToken) || user.RefreshToken.ExpiresOn > DateTime.Now)
                {
                    return new AutRefreshTokenResponse();
                }

                user.RefreshToken = new RefreshToken
                {
                    Token = HashGenerator.GeneratedRefreshToken(),
                    ExpiresOn = DateTime.Now.AddDays(7)
                };

                return new AutRefreshTokenResponse
                {
                    RefreshToken = user.RefreshToken.Token,
                    ExpiresOn = user.RefreshToken.ExpiresOn
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserLogoutResponse> Logout(string userName, string refreshToken)
        {
            var user = await _context.AuthTokens.SingleOrDefaultAsync(element => element.Username == userName);

            if (user == null || !user.RefreshToken.Equals(refreshToken) || user.RefreshToken.ExpiresOn > DateTime.Now)
            {
                return new UserLogoutResponse();
            }

            user.RefreshToken.ExpiresOn = DateTime.Now;

            return new UserLogoutResponse()
            {
                Username = userName,
                Message = "Logout successfully"
            };
        }
    }
}