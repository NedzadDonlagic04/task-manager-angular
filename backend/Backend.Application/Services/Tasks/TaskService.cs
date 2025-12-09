using Backend.Application.DTOs.Task;
using Backend.Application.Errors.Tasks;
using Backend.Application.Errors.Users;
using Backend.Application.Interfaces.Database;
using Backend.Application.Interfaces.Tasks;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Entities.Users;
using Backend.Domain.Enums;
using Backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.Tasks;

public sealed class TaskService(IAppDbContext context) : ITaskService
{
    public async Task<IEnumerable<TaskReadDTO>> GetTasksAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default
    )
    {
        var results = await context
            .Set<TaskEntity>()
            .AsNoTracking()
            .Where(task => task.UserId == ownerId)
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
        TaskGetByIdDTO taskGetByIdDTO,
        CancellationToken cancellationToken = default
    )
    {
        var task = await context
            .Set<TaskEntity>()
            .AsNoTracking()
            .Where(task =>
                task.UserId == taskGetByIdDTO.OwnerId && task.Id == taskGetByIdDTO.TaskId
            )
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
            .FirstOrDefaultAsync(cancellationToken);

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

        var doesUserExist = await context
            .Set<UserEntity>()
            .AnyAsync(user => user.Id == taskCreateDTO.OwnerId, cancellationToken);

        if (!doesUserExist)
        {
            return UserError.NotFound;
        }

        var newTask = new TaskEntity
        {
            Title = taskCreateDTO.Title,
            Description = taskCreateDTO.Description,
            Deadline = taskCreateDTO.Deadline?.ToUniversalTime(),
            TaskStateId = (int)TaskStateEnum.Pending,
            Tags = tags,
            UserId = taskCreateDTO.OwnerId,
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
            .Where(task => task.UserId == taskUpdateDTO.OwnerId && task.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

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
        TaskDeleteByIdDTO taskDeleteByIdDTO,
        CancellationToken cancellationToken = default
    )
    {
        var task = await context
            .Set<TaskEntity>()
            .FirstOrDefaultAsync(
                task =>
                    task.UserId == taskDeleteByIdDTO.OwnerId && task.Id == taskDeleteByIdDTO.TaskId,
                cancellationToken
            );

        if (task is null)
        {
            return TaskError.NotFound;
        }

        context.Set<TaskEntity>().Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteTasksAsync(
        TaskBulkDeleteDTO taskBulkDeleteDTO,
        CancellationToken cancellationToken = default
    )
    {
        if (taskBulkDeleteDTO.TaskIds.Count == 0)
        {
            return TaskError.IdsMissing;
        }

        var tasks = await context
            .Set<TaskEntity>()
            .Where(task =>
                task.UserId == taskBulkDeleteDTO.OwnerId
                && taskBulkDeleteDTO.TaskIds.Contains(task.Id)
            )
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
