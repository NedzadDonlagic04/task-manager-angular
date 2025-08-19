using Microsoft.AspNetCore.Mvc;
using DbContexts;
using DTOs;
using Enums;
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

            await _context.Entry(task).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(task).Reference(t => t.TaskState).LoadAsync();

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
        public async Task<ActionResult<TaskReadDTO>> CreateTask(TaskCreateDTO taskCreateDTO)
        {
            if (taskCreateDTO == null)
            {
                return BadRequest("Task data cannot be null.");
            }

            var tags = await _context.Tag.Where(tag => taskCreateDTO.TagIds.Contains(tag.Id)).ToListAsync();

            if (tags.Count != taskCreateDTO.TagIds.Count)
            {
                return BadRequest("One or more tags do not exist");
            }

            var newTask = new Models.Task
            {
                Title = taskCreateDTO.Title,
                Description = taskCreateDTO.Description,
                Deadline = taskCreateDTO.Deadline?.ToUniversalTime(),
                TaskStateId = (int)TaskStateEnum.Pending,
                Tags = tags
            };

            _context.Task.Add(newTask);
            await _context.SaveChangesAsync();

            await _context.Entry(newTask).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(newTask).Reference(t => t.TaskState).LoadAsync();

            var result = new TaskReadDTO
            {
                Id = newTask.Id,
                Title = newTask.Title,
                Description = newTask.Description,
                Deadline = newTask.Deadline,
                Created_At = newTask.Created_At,
                TaskStateName = newTask.TaskState.Name,
                TagNames = newTask.Tags.Select(tag => tag.Name).ToList()
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(Guid id, TaskUpdateDTO taskUpdateDTO)
        {
            var taskToUpdate = await _context.Task.FindAsync(id);

            if (taskToUpdate == null)
            {
                return NotFound();
            }

            await _context.Entry(taskToUpdate).Collection(t => t.Tags).LoadAsync();
            await _context.Entry(taskToUpdate).Reference(t => t.TaskState).LoadAsync();

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

            taskToUpdate.Title = taskUpdateDTO.Title;
            taskToUpdate.Description = taskUpdateDTO.Description;
            taskToUpdate.Deadline = taskUpdateDTO.Deadline;
            taskToUpdate.TaskStateId = taskUpdateDTO.TaskStateId;

            taskToUpdate.Tags.Clear();
            foreach (Models.Tag tag in tags)
            {
                taskToUpdate.Tags.Add(tag);
            }

            _context.Task.Update(taskToUpdate);
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
