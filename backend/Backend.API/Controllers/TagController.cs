using Backend.Application.DTOs;
using Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/tag")]
public sealed class TagController(ITagService tagService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags(
        CancellationToken cancellationToken
    )
    {
        var tags = await tagService.GetTagsAsync(cancellationToken);

        return Ok(tags);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TagDTO>> GetTagById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var tag = await tagService.GetTagByIdAsync(id, cancellationToken);

        return tag.IsFailure
            ? (ActionResult<TagDTO>)NotFound()
            : (ActionResult<TagDTO>)Ok(tag.Value);
    }
}
