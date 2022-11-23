namespace Auth.Storage.Entities
{
    public class AuthToken
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public Guid ResourceId { get; set; } = Guid.NewGuid();
        public virtual Resource Resource { get; set; } = new Resource();
        public string Hash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public RefreshToken RefreshToken { get; set; } = new RefreshToken();
    }
}