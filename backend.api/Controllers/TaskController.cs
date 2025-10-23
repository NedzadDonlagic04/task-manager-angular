using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/task")]
public sealed class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskReadDTO>>> GetTasks()
    {
        var tasks = await _taskService.GetTasksAsync();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDTO>> GetTaskById([FromRoute] Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task.IsFailure)
        {
            return NotFound();
        }

        return Ok(task.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskReadDTO>> CreateTask([FromBody] TaskCreateUpdateDTO taskCreateDTO)
    {
        var createdTask = await _taskService.CreateTaskAsync(taskCreateDTO);

        if (createdTask.IsFailure)
        {
            return BadRequest(createdTask.Errors);
        }

        return Ok(createdTask.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateTask([FromRoute] Guid id, [FromBody] TaskCreateUpdateDTO taskUpdateDTO)
    {
        var updatedTask = await _taskService.UpdateTaskAsync(id, taskUpdateDTO);

        if (updatedTask.IsFailure)
        {
            return BadRequest(updatedTask.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTask([FromRoute] Guid id)
    {
        var result = await _taskService.DeleteTask(id);

        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteMultipleTasks([FromBody] List<Guid> ids)
    {
        var result = await _taskService.DeleteTasks(ids);

        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}
