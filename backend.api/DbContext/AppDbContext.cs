using Enums;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

            modelBuilder.Entity<TaskState>().HasData(taskStatesData);

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
}
