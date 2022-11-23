namespace JwtAuth.API.APIModels
{
    public class AuthTokenResponse
    {
        public string Username { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
        public int Duration { get; set; }
    }
}