using Backend.Application.DTOs.Task;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Tasks;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDTO>> GetTasksAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default
    );
    Task<Result<TaskReadDTO>> GetTaskByIdAsync(
        TaskGetByIdDTO taskGetByIdDTO,
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
    Task<Result> DeleteTaskAsync(
        TaskDeleteByIdDTO taskDeleteByIdDTO,
        CancellationToken cancellationToken = default
    );
    Task<Result> DeleteTasksAsync(
        TaskBulkDeleteDTO taskBulkDeleteDTO,
        CancellationToken cancellationToken = default
    );
}
