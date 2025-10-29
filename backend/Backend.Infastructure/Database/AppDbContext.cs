using System.Reflection;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infastructure.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options),
        IAppDbContext
{
    public DbSet<Tag> Tag { get; private set; } = null!;
    public DbSet<Domain.Entities.Task> Task { get; private set; } = null!;
    public DbSet<TaskState> TaskState { get; private set; } = null!;
    public DbSet<TaskTag> TaskTag { get; private set; } = null!;
    public DbSet<User> User { get; private set; } = null!;
    public DbSet<UserProfile> UserProfile { get; private set; } = null!;

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

        modelBuilder.Entity<TaskTag>().HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });
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
