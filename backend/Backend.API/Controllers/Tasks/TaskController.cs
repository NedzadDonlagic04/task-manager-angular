using Backend.API.Abstracts;
using Backend.API.Extensions;
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
        var tasks = await taskService.GetTasksAsync(User.GetUserId(), cancellationToken);

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
        var taskGetByIdDTO = new TaskGetByIdDTO { TaskId = id, OwnerId = User.GetUserId() };

        var task = await taskService.GetTaskByIdAsync(taskGetByIdDTO, cancellationToken);

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
        taskCreateDTO.OwnerId = User.GetUserId();

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
        taskUpdateDTO.OwnerId = User.GetUserId();

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
        var taskDeleteByIdDTO = new TaskDeleteByIdDTO { TaskId = id, OwnerId = User.GetUserId() };

        var result = await taskService.DeleteTaskAsync(taskDeleteByIdDTO, cancellationToken);

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
        var taskBulkDeleteDTO = new TaskBulkDeleteDTO { OwnerId = User.GetUserId(), TaskIds = ids };

        var result = await taskService.DeleteTasksAsync(taskBulkDeleteDTO, cancellationToken);

        return result.IsFailure ? Problem(result.Errors) : NoContent();
    }
}
