using Auth.Storage.Enums;

namespace Auth.Storage.Entities
{
    public class Resource
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public ResourceType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual List<AuthToken> AuthTokens { get; set; } = null!;
    }
}