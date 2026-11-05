using Microsoft.EntityFrameworkCore;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Infrastructure.Database;

/// <summary>
/// Контекст базы данных
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PasswordSalt).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);
            entity.Property(e => e.IsEmailConfirmed).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
            entity.Metadata
                .FindNavigation(nameof(User.Devices))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new DeviceId(value))
                .ValueGeneratedNever();
            entity.Property(e => e.OwnerId)
                .HasConversion(
                    id => id.Value,  
                    value => new UserId(value)) 
                .IsRequired();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PlatformType).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);

            entity.HasIndex(e => e.OwnerId);
            entity.HasIndex(e => e.Status);
            
            entity.Property<Guid>("OwnerIdValue").IsRequired();
            entity.HasIndex("OwnerIdValue");

            entity.HasOne<User>()
                .WithMany(u => u.Devices)
                .HasForeignKey("OwnerIdValue")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    public override int SaveChanges()
    {
        SyncShadowProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SyncShadowProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SyncShadowProperties()
    {
        foreach (var entry in ChangeTracker.Entries<Device>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Property("OwnerIdValue").CurrentValue = entry.Entity.OwnerId.Value;
            }
        }
    }
}
