using DbContexts;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Services;

public class TaskStateService : ITaskStateService
{
    private readonly AppDbContext _context;

    public TaskStateService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync()
    {
        var results = await _context
                            .TaskState
                            .AsNoTracking()
                            .Select(taskState => new TaskStateDTO
                            {
                                Id = taskState.Id,
                                Name = taskState.Name
                            })
                            .ToListAsync();

        return results;
    }

    public async Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(int id)
    {
        var taskState = await _context
                               .TaskState
                               .AsNoTracking()
                               .Select(taskState => new TaskStateDTO
                               {
                                   Id = taskState.Id,
                                   Name = taskState.Name
                               })
                               .FirstOrDefaultAsync(taskState => taskState.Id == id);

        if (taskState == null)
        {
            return Result<TaskStateDTO>.Failure("Task state not found");
        }

        return Result<TaskStateDTO>.Success(taskState);
    }
}
