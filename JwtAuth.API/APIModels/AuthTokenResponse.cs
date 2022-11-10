namespace JwtAuth.API.APIModels
{
    public class AuthTokenResponse
    {
        public string UserName { get; set; } = String.Empty;
        public DateTime CreatedOn { get; }
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
        public int Duration { get; set; }
    }
}