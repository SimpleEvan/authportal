namespace JwtAuth.API.APIModels
{
    public class AuthTokenResponse
    {
        public string UserName { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int Duration { get; set; }
    }
}