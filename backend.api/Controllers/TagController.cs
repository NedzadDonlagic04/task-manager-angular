using Microsoft.AspNetCore.Mvc;
using DbContexts;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags()
        {
            var tags = await _context
                            .Tag
                            .Select(tag => new TagDTO
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            })
                            .ToListAsync();

            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTagById(Guid id)
        {
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            var result = new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return Ok(result);
        }
    }
}
