using DTOs;

using Utils;

namespace Services
{
    public interface ITaskStateService
    {
        Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync();
        Task<Result<TaskStateDTO>> GetTaskStateByIdAsync(int id);
    }
}