using Backend.API.Abstracts;
using Backend.Application.DTOs.TaskState;
using Backend.Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Tasks;

[Route("api/task-state")]
public sealed class TaskStateController(ITaskStateService taskStateService) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskStateDTO>>> GetTaskStates(
        CancellationToken cancellationToken = default
    )
    {
        var taskStates = await taskStateService.GetTaskStatesAsync(cancellationToken);

        return Ok(taskStates);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskStateDTO>> GetTaskStateById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        var taskState = await taskStateService.GetTaskStateByIdAsync(id, cancellationToken);

        return taskState.IsFailure ? Problem(taskState.Errors) : Ok(taskState.Value);
    }
}
