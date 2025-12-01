using System.Reflection;
using Backend.Application.Interfaces;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Entities.Users;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options),
        IAppDbContext
{
    public DbSet<TagEntity> Tag { get; private set; } = null!;
    public DbSet<TaskEntity> Task { get; private set; } = null!;
    public DbSet<TaskStateEntity> TaskState { get; private set; } = null!;
    public DbSet<TaskTagEntity> TaskTag { get; private set; } = null!;
    public DbSet<UserEntity> User { get; private set; } = null!;
    public DbSet<UserProfileEntity> UserProfile { get; private set; } = null!;

    public override int SaveChanges()
    {
        PrepareEntitiesForSave();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PrepareEntitiesForSave();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder
            .Entity<TaskTagEntity>()
            .HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });
    }

    private void PrepareEntitiesForSave()
    {
        foreach (var timeStampedEntry in ChangeTracker.Entries<ITimeStampedEntity>())
        {
            switch (timeStampedEntry.State)
            {
                case EntityState.Added:
                    timeStampedEntry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    timeStampedEntry.Entity.UpdatedAt = null;
                    break;
                case EntityState.Modified:
                    timeStampedEntry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }
    }
}
