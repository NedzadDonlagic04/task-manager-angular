using DTOs;

using Utils;

namespace Services;

public interface ITaskStateService
{
    Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(CancellationToken cancellationToken);
    Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(int id, CancellationToken cancellationToken);
}