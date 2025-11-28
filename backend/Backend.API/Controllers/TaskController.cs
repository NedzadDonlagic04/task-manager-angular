using Backend.Application.DTOs.TaskDTO;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/task")]
public sealed class TaskController(ITaskService taskService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskReadDTO>>> GetTasks(
        CancellationToken cancellationToken
    )
    {
        var tasks = await taskService.GetTasksAsync(cancellationToken);

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDTO>> GetTaskById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var task = await taskService.GetTaskByIdAsync(id, cancellationToken);

        return task.IsFailure
            ? (ActionResult<TaskReadDTO>)NotFound()
            : (ActionResult<TaskReadDTO>)Ok(task.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskReadDTO>> CreateTask(
        [FromBody] TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken
    )
    {
        var createdTask = await taskService.CreateTaskAsync(taskCreateDTO, cancellationToken);

        return createdTask.IsFailure
            ? (ActionResult<TaskReadDTO>)BadRequest(createdTask.Errors)
            : (ActionResult<TaskReadDTO>)Ok(createdTask.Value);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateTask(
        [FromRoute] Guid id,
        [FromBody] TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken
    )
    {
        var updatedTask = await taskService.UpdateTaskAsync(id, taskUpdateDTO, cancellationToken);

        return updatedTask.IsFailure ? BadRequest(updatedTask.Errors) : NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTask(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var result = await taskService.DeleteTaskAsync(id, cancellationToken);

        return result.IsFailure ? BadRequest(result.Errors) : NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteMultipleTasks(
        [FromBody] List<Guid> ids,
        CancellationToken cancellationToken
    )
    {
        var result = await taskService.DeleteTasksAsync(ids, cancellationToken);

        return result.IsFailure ? BadRequest(result.Errors) : NoContent();
    }
}
