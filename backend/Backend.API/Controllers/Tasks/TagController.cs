using Backend.API.Abstracts;
using Backend.Application.DTOs.Tag;
using Backend.Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Tasks;

[Route("api/tag")]
public sealed class TagController(ITagService tagService) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags(
        CancellationToken cancellationToken = default
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
        CancellationToken cancellationToken = default
    )
    {
        var tag = await tagService.GetTagByIdAsync(id, cancellationToken);

        return tag.IsFailure ? Problem(tag.Errors) : Ok(tag.Value);
    }
}
