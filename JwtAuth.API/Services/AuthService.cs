﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public async Task<AuthTokenResponse> Register(UserRequest userRequest)
        {
            var hashObj = HashGenerator.CreateHash(userRequest.Password);

            // store salt & hash in the db

            return new AuthTokenResponse
            {
                UserName = userRequest.Username,
                CreatedOn = DateTime.Now,
                ExpiresAt = DateTime.Now.AddSeconds(3600),
                Duration = 3600
            };
        }

        public async Task<string> Login(UserRequest userRequest)
        {
            var hashObj = HashGenerator.CreateHash(userRequest.Password);

            //hashObj.hash.SequenceEqual(storedHash);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userRequest.Username)
            };

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
            };
        }
    }
}
