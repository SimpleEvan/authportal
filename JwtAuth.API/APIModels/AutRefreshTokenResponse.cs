namespace JwtAuth.API.APIModels
{
    public class AutRefreshTokenResponse
    {
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime ExpiresOn { get; set; } = DateTime.MinValue;
    }
}