using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Backend.Domain.Models;
using Backend.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services;

public sealed class TaskStateService(IAppDbContext context) : ITaskStateService
{
    public async Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(
        CancellationToken cancellationToken
    )
    {
        var results = await context
            .Set<TaskState>()
            .AsNoTracking()
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
            .Set<TaskState>()
            .AsNoTracking()
            .Select(taskState => new TaskStateDTO { Id = taskState.Id, Name = taskState.Name })
            .FirstOrDefaultAsync(taskState => taskState.Id == id, cancellationToken);

        return taskState == null
            ? Result<TaskStateDTO>.Failure("Task state not found")
            : Result<TaskStateDTO>.Success(taskState);
    }
}
