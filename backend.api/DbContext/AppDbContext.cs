using Enums;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DbContexts;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskTag>()
                    .HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });

        var taskStatesData = Enum.GetValues(typeof(TaskStateEnum))
            .Cast<TaskStateEnum>()
            .Select(
                taskStateEnum => new TaskState
                {
                    Id = (int)taskStateEnum,
                    Name = taskStateEnum.ToString()
                }
            )
            .ToArray();

        List<Models.Tag> tagData = [
            new Models.Tag { Id = Guid.Parse("e280113f-a704-4e96-b909-30b322cc08b4"), Name = "job"  },
                new Models.Tag { Id = Guid.Parse("1ea6c135-be0f-4cd8-8029-863975601330"), Name = "hobby"  },
                new Models.Tag { Id = Guid.Parse("345b41fc-0019-46e0-b35d-e5a61ad76a4b"), Name = "school" },
                new Models.Tag { Id = Guid.Parse("ea56ef56-0c7b-4995-9f7f-1333a363a9db"), Name = "house" }
        ];

        modelBuilder.Entity<TaskState>().HasData(taskStatesData);
        modelBuilder.Entity<Tag>().HasData(tagData);

        modelBuilder.Entity<Models.Task>()
                    .HasMany(task => task.Tags)
                    .WithMany(tag => tag.Tasks)
                    .UsingEntity<TaskTag>();
    }

    public DbSet<Tag> Tag { get; private set; } = null!;
    public DbSet<Models.Task> Task { get; private set; } = null!;
    public DbSet<TaskState> TaskState { get; private set; } = null!;
    public DbSet<TaskTag> TaskTag { get; private set; } = null!;
}
