using DTOs;
using Utils;

namespace Services;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDTO>> GetTasksAsync();
    Task<Result<TaskReadDTO>> GetTaskByIdAsync(Guid id);
    Task<Result<TaskReadDTO>> CreateTaskAsync(TaskCreateUpdateDTO taskCreateDTO);
    Task<Result<TaskReadDTO>> UpdateTaskAsync(Guid id, TaskCreateUpdateDTO taskUpdateDTO);
    Task<Result> DeleteTask(Guid id);
    Task<Result> DeleteTasks(List<Guid> ids);
}
