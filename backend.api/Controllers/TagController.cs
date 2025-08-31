using Microsoft.AspNetCore.Mvc;
using DTOs;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags()
        {
            var tags = await _tagService.GetTagsAsync();

            return Ok(tags);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDTO>> GetTagById([FromRoute] Guid id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }
    }
}
