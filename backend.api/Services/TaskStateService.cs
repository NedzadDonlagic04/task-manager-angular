using DTOs;
using DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Services {
    public class TaskStateService : ITaskStateService {
        private readonly AppDbContext _context;

        public TaskStateService(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TaskStateDTO>> GetTaskStatesAsync() {
            var results = await _context
                                .TaskState
                                .Select(taskState => new TaskStateDTO()
                                {
                                    Id = taskState.Id,
                                    Name = taskState.Name
                                })
                                .ToListAsync();

            return results;
        }

        public async Task<TaskStateDTO?> GetTaskStateByIdAsync(int id) {
            var taskState = await _context.TaskState.FindAsync(id);

            if (taskState == null) {
                return null;
            }

            var result = new TaskStateDTO()
            {
                Id = taskState.Id,
                Name = taskState.Name
            };

            return result;
        }
    }
}
