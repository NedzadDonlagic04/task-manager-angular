using DTOs;

namespace Services {
    public interface ITaskStateService {
        Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync();
        Task<TaskStateDTO?> GetTaskStateByIdAsync(int id);
    }
}
