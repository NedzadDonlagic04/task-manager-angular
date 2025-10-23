using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/task-state")]
public sealed class TaskStateController(ITaskStateService taskStateService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskStateDTO>>> GetTaskStates()
    {
        var taskStates = await taskStateService.GetTaskStatesAsync();

        return Ok(taskStates);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskStateDTO>> GetTaskStateById([FromRoute] int id)
    {
        var taskState = await taskStateService.GetTaskStateByIdAsync(id);

        if (taskState.IsFailure)
        {
            return NotFound();
        }

        return Ok(taskState.Value);
    }
}
