using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace Controllers
{
    [ApiController]
    [Route("api/task-state")]
    public class TaskStateController : ControllerBase
    {
        private ITaskStateService _taskStateService;

        public TaskStateController(ITaskStateService taskStateService) {
            _taskStateService = taskStateService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TaskStateDTO>>> GetTaskStates()
        {
            var taskStates = await _taskStateService.GetTaskStatesAsync();

            return Ok(taskStates);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskStateDTO>> GetTaskStateById([FromRoute] int id)
        {
            var taskState = await _taskStateService.GetTaskStateByIdAsync(id);

            if (taskState == null)
            {
                return NotFound();
            }

            return Ok(taskState);
        }
    }
}
