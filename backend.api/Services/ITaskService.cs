using DTOs;
using Utils;

namespace Services {
    public interface ITaskService {
        Task<IEnumerable<TaskReadDTO>> GetTasksAsync();
        Task<Result<TaskReadDTO>> GetTaskByIdAsync(Guid id);
        Task<Result<TaskReadDTO>> CreateTaskAsync(TaskCreateDTO taskCreateDTO);
        Task<Result<TaskReadDTO>> UpdateTaskAsync(Guid id, TaskUpdateDTO taskUpdateDTO);
        Task<Result> DeleteTask(Guid id);
        Task<Result> DeleteTasks(List<Guid> ids);
    }
}
