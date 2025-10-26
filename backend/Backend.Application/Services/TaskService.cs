using Backend.Application.DTOs.TaskDTO;
using Backend.Application.Interfaces;
using Backend.Domain.Enums;
using Backend.Domain.Models;
using Backend.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services;

public sealed class TaskService(IAppDbContext context) : ITaskService
{
    public async Task<IEnumerable<TaskReadDTO>> GetTasksAsync(CancellationToken cancellationToken)
    {
        var results = await context
            .Set<Domain.Models.Task>()
            .AsNoTracking()
            .Include(task => task.Tags)
            .Include(task => task.TaskState)
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
        CancellationToken cancellationToken
    )
    {
        var task = await context
            .Set<Domain.Models.Task>()
            .AsNoTracking()
            .Include(task => task.Tags)
            .Include(task => task.TaskState)
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

        return task == null
            ? Result<TaskReadDTO>.Failure("Task not found")
            : Result<TaskReadDTO>.Success(task);
    }

    public async Task<Result<TaskReadDTO>> CreateTaskAsync(
        TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken
    )
    {
        var tags = await context
            .Set<Tag>()
            .Where(tag => taskCreateDTO.TagIds.Contains(tag.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count != taskCreateDTO.TagIds.Count)
        {
            return Result<TaskReadDTO>.Failure("One or more tags do not exist");
        }

        var newTask = new Domain.Models.Task
        {
            Title = taskCreateDTO.Title,
            Description = taskCreateDTO.Description,
            Deadline = taskCreateDTO.Deadline?.ToUniversalTime(),
            TaskStateId = (int)TaskStateEnum.Pending,
            Tags = tags,
        };

        context.Set<Domain.Models.Task>().Add(newTask);
        await context.SaveChangesAsync(cancellationToken);
        await context
            .Set<Domain.Models.Task>()
            .Entry(newTask)
            .Reference(task => task.TaskState)
            .LoadAsync(cancellationToken);
        await context
            .Set<Domain.Models.Task>()
            .Entry(newTask)
            .Collection(task => task.Tags)
            .LoadAsync(cancellationToken);

        var result = new TaskReadDTO
        {
            Id = newTask.Id,
            Title = newTask.Title,
            Description = newTask.Description,
            Deadline = newTask.Deadline,
            CreatedAt = newTask.CreatedAt,
            TaskStateName = newTask.TaskState.Name,
            TagNames = newTask.Tags.Select(tag => tag.Name).ToList(),
        };

        return Result<TaskReadDTO>.Success(result);
    }

    public async Task<Result<TaskReadDTO>> UpdateTaskAsync(
        Guid id,
        TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken
    )
    {
        var taskToUpdate = await context
            .Set<Domain.Models.Task>()
            .Include(task => task.Tags)
            .Include(task => task.TaskState)
            .FirstOrDefaultAsync(task => task.Id == id, cancellationToken);

        if (taskToUpdate == null)
        {
            return Result<TaskReadDTO>.Failure("Task to update doesn't exist");
        }

        var tags = await context
            .Set<Tag>()
            .Where(tag => taskUpdateDTO.TagIds.Contains(tag.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count != taskUpdateDTO.TagIds.Count)
        {
            return Result<TaskReadDTO>.Failure("One or more tags do not exist");
        }

        taskToUpdate.Title = taskUpdateDTO.Title;
        taskToUpdate.Description = taskUpdateDTO.Description;
        taskToUpdate.Deadline = taskUpdateDTO.Deadline;

        taskToUpdate.Tags.Clear();
        foreach (Tag tag in tags)
        {
            taskToUpdate.Tags.Add(tag);
        }

        await context
            .Set<Domain.Models.Task>()
            .Entry(taskToUpdate)
            .Reference(task => task.TaskState)
            .LoadAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var updatedTask = new TaskReadDTO()
        {
            Id = taskToUpdate.Id,
            Title = taskToUpdate.Title,
            Description = taskToUpdate.Description,
            Deadline = taskToUpdate.Deadline,
            CreatedAt = taskToUpdate.CreatedAt,
            TaskStateName = taskToUpdate.TaskState.Name,
            TagNames = taskToUpdate.Tags.Select(tag => tag.Name).ToList(),
        };

        return Result<TaskReadDTO>.Success(updatedTask);
    }

    public async Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await context.Set<Domain.Models.Task>().FindAsync([id], cancellationToken);

        if (task == null)
        {
            return Result.Failure("Task to delete doesn't exist");
        }

        context.Set<Domain.Models.Task>().Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteTasksAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return Result.Failure("No task IDs provided");
        }

        var tasks = await context
            .Set<Domain.Models.Task>()
            .Where(task => ids.Contains(task.Id))
            .ToListAsync(cancellationToken);

        if (tasks.Count == 0)
        {
            return Result.Failure("No tasks found for given list of ids");
        }

        context.Set<Domain.Models.Task>().RemoveRange(tasks);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
