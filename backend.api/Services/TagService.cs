using DTOs;
using DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Services {
    public class TagService : ITagService {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TagDTO>> GetTagsAsync() {
            var results = await _context
                                .Tag
                                .Select(tag => new TagDTO()
                                {
                                    Id = tag.Id,
                                    Name = tag.Name
                                })
                                .ToListAsync();

            return results;
        }

        public async Task<TagDTO?> GetTagByIdAsync(Guid id) {
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null) {
                return null;
            }

            var result = new TagDTO()
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return result;
        }
    }
}
