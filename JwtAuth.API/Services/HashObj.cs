namespace JwtAuth.API.Services
{
    public class HashObj
    {
        public byte[] hash { get; set; } = null!;
        public byte[] salt { get; set; } = null!;
    }
}