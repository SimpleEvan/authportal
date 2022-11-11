using Auth.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Storage.Context
{
    public class AuthContext: DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
        : base(options){}

        public AuthContext(){}

        public virtual DbSet<AuthToken> AuthTokens { get; set; } = null!;
        public virtual DbSet<Resource> Resources { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthToken>()
                .ToContainer("tokens")
                .HasPartitionKey(entity => entity.id);

            modelBuilder.Entity<Resource>()
                .ToContainer("resources")
                .HasPartitionKey(entity => entity.id);
        }   
    }
}
    