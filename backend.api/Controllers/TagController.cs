using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/tag")]
public sealed class TagController : ControllerBase
{
    private readonly ITagService _tagService;

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

        if (tag.IsFailure)
        {
            return NotFound();
        }

        return Ok(tag.Value);
    }
}
