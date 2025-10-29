using Backend.Application.DTOs;
using Backend.Shared.Classes;

namespace Backend.Application.Services;

public interface ITaskStateService
{
    Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync(CancellationToken cancellationToken);
    Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(int id, CancellationToken cancellationToken);
}
