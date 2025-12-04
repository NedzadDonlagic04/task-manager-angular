using Backend.Application.DTOs.Task;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Tasks;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDTO>> GetTasksAsync(CancellationToken cancellationToken = default);
    Task<Result<TaskReadDTO>> GetTaskByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
    Task<Result<TaskReadDTO>> CreateTaskAsync(
        TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken = default
    );
    Task<Result<TaskReadDTO>> UpdateTaskAsync(
        Guid id,
        TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken = default
    );
    Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeleteTasksAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
