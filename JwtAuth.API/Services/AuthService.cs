using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Storage.Context;
using Auth.Storage.Entities;
using Auth.Storage.Enums;
using JwtAuth.API.APIModels;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.API.Services
{
    public interface IAuthService
    {
        Task<AuthTokenResponse> Register(UserRequest user);
        Task<string> Login(UserRequest userRequest);
        Task<UserLoggedResponse> Logout(UserRequest userRequest);
    }

    public class AuthService: IAuthService
    {
        private readonly AuthContext _context;

        public AuthService(AuthContext context)
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
            var hashObj = HashGenerator.CreateHash(userRequest.Password);

            //hashObj.hash.SequenceEqual(storedHash);

            //#TODO  check is hash is correct from db 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userRequest.Username)
            };

            // get from keyvault

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tokenvalue1111111"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt; 
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

