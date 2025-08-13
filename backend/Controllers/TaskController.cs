using Microsoft.AspNetCore.Mvc;
using DbContexts;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskReadDTO>>> GetTasks()
        {
            var tasks = await _context
                            .Task
                            .Include(task => task.Tags)
                            .Include(task => task.TaskState)
                            .Select(task => new TaskReadDTO
                            {
                                Id = task.Id,
                                Title = task.Title,
                                Description = task.Description,
                                Deadline = task.Deadline,
                                Created_At = task.Created_At,
                                TaskStateName = task.TaskState.Name,
                                TagNames = task.Tags.Select(tag => tag.Name).ToList()
                            })
                            .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTaskById(Guid id)
        {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var result = new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                Created_At = task.Created_At,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TaskReadDTO>> CreateTask(TaskCreateUpdateDTO taskCreateDTO)
        {
            var taskState = await _context.TaskState.FindAsync(taskCreateDTO.TaskStateId);

            if (taskState == null)
            {
                return BadRequest($"TaskState with id {taskCreateDTO.TaskStateId} does not exist");
            }

            var tags = await _context.Tag.Where(tag => taskCreateDTO.TagIds.Contains(tag.Id)).ToListAsync();

            if (tags.Count != taskCreateDTO.TagIds.Count)
            {
                return BadRequest("One or more tags do not exist");
            }

            var task = new Models.Task
            {
                Title = taskCreateDTO.Title,
                Description = taskCreateDTO.Description,
                Deadline = taskCreateDTO.Deadline,
                TaskStateId = taskCreateDTO.TaskStateId,
                Tags = tags
            };

            _context.Task.Add(task);
            await _context.SaveChangesAsync();

            var result = new TaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                Created_At = task.Created_At,
                TaskStateName = task.TaskState.Name,
                TagNames = task.Tags.Select(tag => tag.Name).ToList()
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(Guid id, TaskCreateUpdateDTO taskUpdateDTO)
        {
            var taskState = await _context.TaskState.FindAsync(taskUpdateDTO.TaskStateId);

            if (taskState == null)
            {
                return BadRequest($"TaskState with id {taskUpdateDTO.TaskStateId} does not exist");
            }

            var tags = await _context.Tag.Where(tag => taskUpdateDTO.TagIds.Contains(tag.Id)).ToListAsync();

            if (tags.Count != taskUpdateDTO.TagIds.Count)
            {
                return BadRequest("One or more tags do not exist");
            }

            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = taskUpdateDTO.Title;
            task.Description = taskUpdateDTO.Description;
            task.Deadline = taskUpdateDTO.Deadline;
            task.TaskStateId = taskUpdateDTO.TaskStateId;
            task.Tags = tags;

            _context.Task.Update(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
