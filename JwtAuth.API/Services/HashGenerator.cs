using System.Security.Cryptography;
using System.Text;

namespace JwtAuth.API.Services
{
    public static class HashGenerator
    {
        public static HashObj CreateHash(string inputValue)
        {
            using (var hmac = new HMACSHA512())
            {
                return new HashObj
                {
                    salt = hmac.Key,
                    hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputValue))
                };
            }
        }
    }
}

