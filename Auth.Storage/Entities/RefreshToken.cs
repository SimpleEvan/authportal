namespace Auth.Storage.Entities
{
    public class RefreshToken
	{
		public string Token { get; set; } = string.Empty;
		public DateTime CreatedOn { get; } = DateTime.Now;
		public DateTime ExpiresOn { get; set; } = DateTime.MinValue;
    }
}