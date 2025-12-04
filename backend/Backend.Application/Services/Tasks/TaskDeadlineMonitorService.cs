using Backend.Application.Interfaces.Database;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Enums;
using Backend.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Application.Services.Tasks;

public sealed class TaskDeadlineMonitorService(
    IServiceProvider serviceProvider,
    ILogger<TaskDeadlineMonitorService> logger,
    IOptions<TaskDeadlineMonitorOptions> taskDeadlineMonitorOptions
) : BackgroundService
{
    private readonly TaskDeadlineMonitorOptions _taskDeadlineMonitorOptions =
        taskDeadlineMonitorOptions.Value;

    private readonly TimeSpan _checkExpiredDeadlineInterval = TimeSpan.FromSeconds(
        taskDeadlineMonitorOptions.Value.CheckExpiredDeadlineSeconds
    );

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var timer = new PeriodicTimer(_checkExpiredDeadlineInterval);

        if (_taskDeadlineMonitorOptions.RunOnStart)
        {
            await SetExpiredTasksToFailed(cancellationToken);
        }

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
                    "Task '{Title}' (ID: {Id}) expired - state changed to {NewState}",
                    expiredTask.Title,
                    expiredTask.Id,
                    nameof(TaskStateEnum.Fail)
                );
            }

            if (expiredTasks.Count != 0)
            {
                await context.SaveChangesAsync(cancellationToken);

                logger.LogInformation(
                    "Expired task state update completed, {Count} task(s) marked as {State}",
                    expiredTasks.Count,
                    nameof(TaskStateEnum.Fail)
                );
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Stopping service...");
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
