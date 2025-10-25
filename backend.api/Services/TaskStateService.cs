using DbContexts;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Services;

public sealed class TaskStateService(AppDbContext context) : ITaskStateService
{
    public async Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(
        CancellationToken cancellationToken
    )
    {
        var results = await context
            .TaskState.AsNoTracking()
            .Select(taskState => new TaskStateDTO { Id = taskState.Id, Name = taskState.Name })
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        var taskState = await context
            .TaskState.AsNoTracking()
            .Select(taskState => new TaskStateDTO { Id = taskState.Id, Name = taskState.Name })
            .FirstOrDefaultAsync(taskState => taskState.Id == id, cancellationToken);

        if (taskState == null)
        {
            return Result<TaskStateDTO>.Failure("Task state not found");
        }

        return Result<TaskStateDTO>.Success(taskState);
    }
}
