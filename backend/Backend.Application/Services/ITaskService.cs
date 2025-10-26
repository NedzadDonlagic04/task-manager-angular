using Backend.Application.DTOs.TaskDTO;
using Backend.Shared.Utils;

namespace Backend.Application.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDTO>> GetTasksAsync(CancellationToken cancellationToken);
    Task<Result<TaskReadDTO>> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<TaskReadDTO>> CreateTaskAsync(
        TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken
    );
    Task<Result<TaskReadDTO>> UpdateTaskAsync(
        Guid id,
        TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken
    );
    Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
    Task<Result> DeleteTasksAsync(List<Guid> ids, CancellationToken cancellationToken);
}
