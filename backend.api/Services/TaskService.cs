using DTOs;
using DbContexts;
using Microsoft.EntityFrameworkCore;
using Utils;
using Enums;

namespace Services {
    public class TaskService : ITaskService {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TaskReadDTO>> GetTasksAsync() {
            var results = await _context
                                .Task
                                .Include(task => task.Tags)
                                .Include(task => task.TaskState)
                                .Select(task => new TaskReadDTO
                                {
                                    Id = task.Id,
                                    Title = task.Title,
                                    Description = task.Description,
                                    Deadline = task.Deadline,
                                    Created_At = task.Created_At,
                                    TaskStateName = task.TaskState.Name,
                                    TagNames = task.Tags.Select(tag => tag.Name).ToList()
                                })
                                .ToListAsync();

            return results;
        }

        public async Task<TaskReadDTO?> GetTaskByIdAsync(Guid id) {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return null;
            }

            await _context.Entry(task).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(task).Reference(t => t.TaskState).LoadAsync();

            var result = new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                Created_At = task.Created_At,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList()
            };

            return result;
        }

        public async Task<Result<TaskReadDTO>> CreateTaskAsync(TaskCreateDTO taskCreateDTO)
        {
            var tags = await _context
                            .Tag
                            .Where(tag => taskCreateDTO.TagIds.Contains(tag.Id))
                            .ToListAsync();

            if (tags.Count != taskCreateDTO.TagIds.Count)
            {
                return Result<TaskReadDTO>.Failure("One or more tags do not exist");
            }

            var newTask = new Models.Task
            {
                Title = taskCreateDTO.Title,
                Description = taskCreateDTO.Description,
                Deadline = taskCreateDTO.Deadline?.ToUniversalTime(),
                TaskStateId = (int)TaskStateEnum.Pending,
                Tags = tags
            };

            _context.Task.Add(newTask);
            await _context.SaveChangesAsync();

            await _context.Entry(newTask).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(newTask).Reference(t => t.TaskState).LoadAsync();

            var result = new TaskReadDTO
            {
                Id = newTask.Id,
                Title = newTask.Title,
                Description = newTask.Description,
                Deadline = newTask.Deadline,
                Created_At = newTask.Created_At,
                TaskStateName = newTask.TaskState.Name,
                TagNames = newTask.Tags.Select(tag => tag.Name).ToList()
            };

            return Result<TaskReadDTO>.Success(result);
        }

        public async Task<Result<TaskReadDTO>> UpdateTaskAsync(Guid id, TaskUpdateDTO taskUpdateDTO)
        {
            var taskToUpdate = await _context
                                    .Task
                                    .FindAsync(id);

            if (taskToUpdate == null)
            {
                return Result<TaskReadDTO>.Failure("Task to update doesn't exist");
            }

            await _context.Entry(taskToUpdate).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(taskToUpdate).Reference(t => t.TaskState).LoadAsync();

            var taskState = await _context.TaskState.FindAsync(taskUpdateDTO.TaskStateId);

            if (taskState == null)
            {
                return Result<TaskReadDTO>.Failure($"TaskState with id {taskUpdateDTO.TaskStateId} does not exist");
            }

            var tags = await _context.Tag.Where(tag => taskUpdateDTO.TagIds.Contains(tag.Id)).ToListAsync();

            if (tags.Count != taskUpdateDTO.TagIds.Count)
            {
                return Result<TaskReadDTO>.Failure("One or more tags do not exist");
            }

            taskToUpdate.Title = taskUpdateDTO.Title;
            taskToUpdate.Description = taskUpdateDTO.Description;
            taskToUpdate.Deadline = taskUpdateDTO.Deadline;
            taskToUpdate.TaskStateId = taskUpdateDTO.TaskStateId;

            taskToUpdate.Tags.Clear();
            foreach (Models.Tag tag in tags)
            {
                taskToUpdate.Tags.Add(tag);
            }

            _context.Task.Update(taskToUpdate);
            await _context.SaveChangesAsync();

            var updatedTask = new TaskReadDTO()
            {
                Id = taskToUpdate.Id,
                Title = taskToUpdate.Title,
                Description = taskToUpdate.Description,
                Deadline = taskToUpdate.Deadline,
                Created_At = taskToUpdate.Created_At,
                TaskStateName = taskToUpdate.TaskState.Name,
                TagNames = taskToUpdate.Tags.Select(tag => tag.Name).ToList()
            };

            return Result<TaskReadDTO>.Success(updatedTask);
        }

        public async Task<Result> DeleteTask(Guid id) {
            var task = await _context
                            .Task
                            .FindAsync(id);

            if (task == null)
            {
                return Result.Failure("Task to delete doesn't exist");
            }

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteTasks(List<Guid> ids) {
            var tasks = await _context
                            .Task
                            .Where(task => ids.Contains(task.Id))
                            .ToListAsync();

            if (tasks.Count == 0)
            {
                return Result.Failure("No tasks found for given list of ids");
            }

            _context.Task.RemoveRange(tasks);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
