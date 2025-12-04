using Backend.API.Abstracts;
using Backend.Application.DTOs.Task;
using Backend.Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Tasks;

[Route("api/task")]
public sealed class TaskController(ITaskService taskService) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskReadDTO>>> GetTasks(
        CancellationToken cancellationToken = default
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
        CancellationToken cancellationToken = default
    )
    {
        var task = await taskService.GetTaskByIdAsync(id, cancellationToken);

        return task.IsFailure ? Problem(task.Errors) : Ok(task.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskReadDTO>> CreateTask(
        [FromBody] TaskCreateUpdateDTO taskCreateDTO,
        CancellationToken cancellationToken = default
    )
    {
        var createdTask = await taskService.CreateTaskAsync(taskCreateDTO, cancellationToken);

        return createdTask.IsFailure
            ? Problem(createdTask.Errors)
            : CreatedAtAction(
                nameof(CreateTask),
                new { id = createdTask.Value.Id },
                createdTask.Value
            );
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateTask(
        [FromRoute] Guid id,
        [FromBody] TaskCreateUpdateDTO taskUpdateDTO,
        CancellationToken cancellationToken = default
    )
    {
        var updatedTask = await taskService.UpdateTaskAsync(id, taskUpdateDTO, cancellationToken);

        return updatedTask.IsFailure ? Problem(updatedTask.Errors) : NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTask(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await taskService.DeleteTaskAsync(id, cancellationToken);

        return result.IsFailure ? Problem(result.Errors) : NoContent();
    }

    [HttpPut("bulk-delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteMultipleTasks(
        [FromBody] List<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        var result = await taskService.DeleteTasksAsync(ids, cancellationToken);

        return result.IsFailure ? Problem(result.Errors) : NoContent();
    }
}
