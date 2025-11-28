using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/task-state")]
public sealed class TaskStateController(ITaskStateService taskStateService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskStateDTO>>> GetTaskStates(
        CancellationToken cancellationToken
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
        CancellationToken cancellationToken
    )
    {
        var taskState = await taskStateService.GetTaskStateByIdAsync(id, cancellationToken);

        return taskState.IsFailure
            ? (ActionResult<TaskStateDTO>)NotFound()
            : (ActionResult<TaskStateDTO>)Ok(taskState.Value);
    }
}
