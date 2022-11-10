using System;
namespace JwtAuth.API.APIModels
{
	public class AuthLoginResponse
	{
		public string JwtToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime RefreshTokenExpiresOn { get; set; } = DateTime.MinValue;
    }
}