using Backend.Application.DTOs.Task;
using Backend.Application.Errors.Tasks;
using Backend.Application.Interfaces.Database;
using Backend.Application.Interfaces.Tasks;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Enums;
using Backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.Tasks;

public sealed class TaskService(IAppDbContext context) : ITaskService
{
    public async Task<IEnumerable<TaskReadDTO>> GetTasksAsync(
        CancellationToken cancellationToken = default
    )
    {
        var results = await context
            .Set<TaskEntity>()
            .AsNoTracking()
            .Select(task => new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList(),
            })
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TaskReadDTO>> GetTaskByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var task = await context
            .Set<TaskEntity>()
            .AsNoTracking()
            .Select(task => new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList(),
            })
            .FirstOrDefaultAsync(task => task.Id == id, cancellationToken);

        return task is null ? TaskError.NotFound : Result<TaskReadDTO>.Success(task);
    }

    public async Task<Result<TaskReadDTO>> CreateTaskAsync(
        TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken = default
    )
    {
        var tags = await context
            .Set<TagEntity>()
            .Where(tag => taskCreateDTO.TagIds.Contains(tag.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count != taskCreateDTO.TagIds.Count)
        {
            return TagError.NotFound;
        }

        var newTask = new TaskEntity
        {
            Title = taskCreateDTO.Title,
            Description = taskCreateDTO.Description,
            Deadline = taskCreateDTO.Deadline?.ToUniversalTime(),
            TaskStateId = (int)TaskStateEnum.Pending,
            Tags = tags,
            /*
             *  NOTE TO FUTURE SELF
             *
             *  Remove the hard coded guid below, this was added because at the time
             *  there was no way to get the user id
             */
            UserId = Guid.Parse("9d07ca30-d8f9-40b7-b922-82f567ec6704"),
        };

        context.Set<TaskEntity>().Add(newTask);
        await context.SaveChangesAsync(cancellationToken);

        var resultTask = await context
            .Set<TaskEntity>()
            .Select(task => new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList(),
            })
            .FirstAsync(task => task.Id == newTask.Id, cancellationToken);

        return Result<TaskReadDTO>.Success(resultTask);
    }

    public async Task<Result<TaskReadDTO>> UpdateTaskAsync(
        Guid id,
        TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken = default
    )
    {
        var taskToUpdate = await context
            .Set<TaskEntity>()
            .Include(task => task.Tags)
            .FirstOrDefaultAsync(task => task.Id == id, cancellationToken);

        if (taskToUpdate is null)
        {
            return TaskError.NotFound;
        }

        var tags = await context
            .Set<TagEntity>()
            .Where(tag => taskUpdateDTO.TagIds.Contains(tag.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count != taskUpdateDTO.TagIds.Count)
        {
            return TagError.NotFound;
        }

        taskToUpdate.Update(
            taskUpdateDTO.Title,
            taskUpdateDTO.Description,
            taskUpdateDTO.Deadline,
            tags
        );
        await context.SaveChangesAsync(cancellationToken);

        var updatedTask = await context
            .Set<TaskEntity>()
            .Select(task => new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList(),
            })
            .FirstAsync(task => task.Id == taskToUpdate.Id, cancellationToken);

        return Result<TaskReadDTO>.Success(updatedTask);
    }

    public async Task<Result> DeleteTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var task = await context.Set<TaskEntity>().FindAsync([id], cancellationToken);

        if (task is null)
        {
            return TaskError.NotFound;
        }

        context.Set<TaskEntity>().Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteTasksAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        if (ids.Count == 0)
        {
            return TaskError.IdsMissing;
        }

        var tasks = await context
            .Set<TaskEntity>()
            .Where(task => ids.Contains(task.Id))
            .ToListAsync(cancellationToken);

        if (tasks.Count == 0)
        {
            return TaskError.NotFound;
        }

        context.Set<TaskEntity>().RemoveRange(tasks);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
