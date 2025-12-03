using Backend.Application.DTOs;
using Backend.Application.Errors.Tasks;
using Backend.Application.Interfaces.Database;
using Backend.Application.Interfaces.Tasks;
using Backend.Domain.Entities.Tasks;
using Backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.Tasks;

public sealed class TaskStateService(IAppDbContext context) : ITaskStateService
{
    public async Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var results = await context
            .Set<TaskStateEntity>()
            .AsNoTracking()
            .Select(taskState => new TaskStateDTO { Id = taskState.Id, Name = taskState.Name })
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var taskState = await context
            .Set<TaskStateEntity>()
            .AsNoTracking()
            .Select(taskState => new TaskStateDTO { Id = taskState.Id, Name = taskState.Name })
            .FirstOrDefaultAsync(taskState => taskState.Id == id, cancellationToken);

        return taskState is null
            ? TaskStateError.NotFound
            : Result<TaskStateDTO>.Success(taskState);
    }
}
