using Backend.Application.DTOs.TaskState;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Tasks;

public interface ITaskStateService
{
    Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(
        CancellationToken cancellationToken = default
    );
    Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
}
