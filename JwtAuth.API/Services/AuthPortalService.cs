using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Enums;
using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.API.Services
{
    public class AuthPortalService : IAuthPortalService
    {
        private readonly AuthContext _context;

        public AuthPortalService(AuthContext context)
        {
            _context = context;
        }

        public async Task<AuthTokenResponse> Register(UserRequest userRequest)
        {
            try
            {
                // #TODO save hash as string not byte array
                var hashObj = HashGenerator.CreateHash(userRequest.Password);

                await _context.AddAsync(new AuthToken
                {
                    Username = userRequest.Username,
                    CreatedOn = DateTime.Now,
                    Duration = 3600,
                    Hash = hashObj.hash,
                    Salt = hashObj.salt,
                    Resource = new Resource
                    {
                        Description = "Register auth token",
                        Type = ResourceType.Tenant
                    }
                });

                await _context.SaveChangesAsync();

                return new AuthTokenResponse
                {
                    UserName = userRequest.Username,
                    CreatedOn = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddSeconds(3600),
                    Duration = 3600
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Login(UserRequest userRequest)
        {
            var user = _context.AuthTokens.SingleOrDefault(element => element.Username == userRequest.Username);

            if (user != null)
            {
                if (!HashGenerator.VerifyPasswordHash(userRequest.Password, user.Hash, user.Salt))
                {
                    return string.Empty;
                }

                var claims = new List<Claim>
                {   
                    new Claim(ClaimTypes.Name, userRequest.Username),
                    new Claim(ClaimTypes.Role, nameof(AuthorizationRoles.Dolphin))
                };

                // get from keyvault

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tokenvalue1111111"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<UserLoggedResponse> Logout(UserRequest userRequest)
        {
            return new UserLoggedResponse
            {
                UserName = userRequest.Username,
                Token = string.Empty
            };
        }
    }
}

