using Backend.Application.DTOs;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces;

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
