using Microsoft.AspNetCore.Mvc;
using DbContexts;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/task-state")]
    public class TaskStateController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskStateController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskStateDTO>>> GetTaskStates()
        {
            var taskStates = await _context
                                    .TaskState
                                    .Select(taskState => new TaskStateDTO
                                    {
                                        Id = taskState.Id,
                                        Name = taskState.Name
                                    })
                                    .ToListAsync();

            return Ok(taskStates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskStateDTO>> GetTaskStateById(Guid id)
        {
            var taskState = await _context.TaskState.FindAsync(id);

            if (taskState == null)
            {
                return NotFound();
            }

            var result = new TaskStateDTO
            {
                Id = taskState.Id,
                Name = taskState.Name
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TaskStateDTO>> CreateTaskState(TaskStateDTO taskStateDTO)
        {
            var taskState = new TaskState
            {
                Name = taskStateDTO.Name
            };

            _context.TaskState.Add(taskState);
            await _context.SaveChangesAsync();

            var result = new TaskStateDTO
            {
                Id = taskState.Id,
                Name = taskState.Name
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTaskState(Guid id, TaskStateDTO taskStateDTO)
        {
            var taskState = await _context.TaskState.FindAsync(id);

            if (taskState == null)
            {
                return NotFound();
            }

            taskState.Name = taskStateDTO.Name;

            _context.TaskState.Update(taskState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskState(Guid id)
        {
            var taskState = await _context.TaskState.FindAsync(id);

            if (taskState == null)
            {
                return NotFound();
            }

            _context.TaskState.Remove(taskState);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
