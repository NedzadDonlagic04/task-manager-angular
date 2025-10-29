using Backend.Application.Interfaces;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Services;

public sealed class TaskDeadlineMonitorService(
    IServiceProvider serviceProvider,
    ILogger<TaskDeadlineMonitorService> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var expiredTasks = await context
                .Set<Domain.Entities.Task>()
                .Where(task =>
                    task.Deadline < DateTime.UtcNow
                    && task.TaskStateId == (int)TaskStateEnum.Pending
                )
                .ToListAsync(stoppingToken);

            foreach (var expiredTask in expiredTasks)
            {
                expiredTask.TaskStateId = (int)TaskStateEnum.Fail;
                logger.LogInformation(
                    "Task '{ExpiredTaskTitle}' (ID: {ExpiredTaskI}) has expired and its state has been set to {TaskStateFailName}.",
                    expiredTask.Title,
                    expiredTask.Id,
                    nameof(TaskStateEnum.Fail)
                );
            }

            if (expiredTasks.Count != 0)
            {
                await context.SaveChangesAsync(stoppingToken);
                logger.LogInformation(
                    "Successfully updated {UpdatedTasksCount} expired tasks.",
                    expiredTasks.Count
                );
            }
            else
            {
                logger.LogInformation("No expired tasks found to update.");
            }

            logger.LogDebug("Waiting for one minute before next check...");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        logger.LogInformation("Task deadline monitoring service is stopping.");
    }
}
