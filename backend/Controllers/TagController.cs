using Microsoft.AspNetCore.Mvc;
using DbContexts;
using DTOs;
using Models;
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

        [HttpPost]
        public async Task<ActionResult<TagDTO>> CreateTag(TagDTO tagDTO)
        {
            var tag = new Tag
            {
                Name = tagDTO.Name
            };

            _context.Tag.Add(tag);
            await _context.SaveChangesAsync();

            var result = new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTag(Guid id, TagDTO tagDTO)
        {
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            tag.Name = tagDTO.Name;

            _context.Tag.Update(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(Guid id)
        {
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
