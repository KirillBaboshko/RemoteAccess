using Microsoft.EntityFrameworkCore;
using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Infrastructure.Database;

public sealed class SignalingDbContext : DbContext
{
    public SignalingDbContext(DbContextOptions<SignalingDbContext> options) : base(options) { }

    public DbSet<SessionHistory> SessionHistory => Set<SessionHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionHistory>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new SessionId(value))
                .ValueGeneratedNever();
            entity.Property(x => x.Version).IsConcurrencyToken();
            entity.HasIndex(x => x.SourceUserId);
            entity.HasIndex(x => x.TargetUserId);
            entity.HasIndex(x => x.CreatedAt);
        });
    }
}
