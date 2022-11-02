namespace Auth.Storage.Entities
{
    public class AuthToken
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid ResourceId { get; set; } = Guid.NewGuid();
        public Resource Resource { get; set; } = new Resource();
        public byte[] Hash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int Duration { get; set; }
    }
}