using DTOs;
using Utils;
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
                                .AsNoTracking()
                                .Select(tag => new TagDTO
                                {
                                    Id = tag.Id,
                                    Name = tag.Name
                                })
                                .ToListAsync();

            return results;
        }

        public async Task<Result<TagDTO>> GetTagByIdAsync(Guid id) {
            var tag = await _context
                            .Tag
                            .AsNoTracking()
                            .Select(tag => new TagDTO
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            })
                            .FirstOrDefaultAsync(tag => tag.Id == id);

            if (tag == null) {
                return Result<TagDTO>.Failure("Tag not found");
            }

            return Result<TagDTO>.Success(tag);
        }
    }
}
