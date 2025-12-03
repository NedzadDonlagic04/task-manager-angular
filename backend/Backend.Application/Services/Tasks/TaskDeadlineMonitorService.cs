using Backend.Application.Interfaces.Database;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Services.Tasks;

public sealed class TaskDeadlineMonitorService(
    IServiceProvider serviceProvider,
    ILogger<TaskDeadlineMonitorService> logger
) : BackgroundService
{
    private static readonly TimeSpan CheckExpiredDeadlineInterval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var timer = new PeriodicTimer(CheckExpiredDeadlineInterval);

        await SetExpiredTasksToFailed(cancellationToken);

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            await SetExpiredTasksToFailed(cancellationToken);
        }
    }

    private async Task SetExpiredTasksToFailed(CancellationToken cancellationToken = default)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var expiredTasks = await context
                .Set<TaskEntity>()
                .Where(task =>
                    task.Deadline < DateTime.UtcNow
                    && task.TaskStateId == (int)TaskStateEnum.Pending
                )
                .ToListAsync(cancellationToken);

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
                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation(
                    "Successfully updated {UpdatedTasksCount} expired tasks.",
                    expiredTasks.Count
                );
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("TaskDeadlineMonitorService stopping...");
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Unexpected error occurred while setting expired tasks to failed"
            );
        }
    }
}
