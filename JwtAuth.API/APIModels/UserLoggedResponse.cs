namespace JwtAuth.API.APIModels
{
    public class UserLoggedResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}