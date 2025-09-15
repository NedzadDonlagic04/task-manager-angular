using DbContexts;

using Enums;

using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TaskDeadlineMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskDeadlineMonitorService> _logger;

        public TaskDeadlineMonitorService(IServiceProvider serviceProvider, ILogger<TaskDeadlineMonitorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var expiredTasks = await context
                                        .Task
                                        .Where(task => task.Deadline < DateTime.UtcNow && task.TaskStateId == (int)TaskStateEnum.Pending)
                                        .ToListAsync(stoppingToken);

                foreach (var expiredTask in expiredTasks)
                {
                    expiredTask.TaskStateId = (int)TaskStateEnum.Fail;
                    _logger.LogInformation("Task '{ExpiredTaskTitle}' (ID: {ExpiredTaskI}) has expired and its state has been set to {TaskStateFailName}.", expiredTask.Title, expiredTask.Id, nameof(TaskStateEnum.Fail));
                }

                if (expiredTasks.Any())
                {
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Successfully updated {UpdatedTasksCount} expired tasks.", expiredTasks.Count);
                }
                else
                {
                    _logger.LogInformation("No expired tasks found to update.");
                }

                _logger.LogDebug("Waiting for one minute before next check...");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            _logger.LogInformation("Task deadline monitoring service is stopping.");
        }
    }
}