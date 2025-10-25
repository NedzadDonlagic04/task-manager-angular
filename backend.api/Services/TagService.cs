using DbContexts;

using DTOs;

using Microsoft.EntityFrameworkCore;

using Utils;

namespace Services;

public sealed class TagService(AppDbContext context) : ITagService
{
    public async Task<IEnumerable<TagDTO>> GetTagsAsync(CancellationToken cancellationToken)
    {
        var results = await context
                            .Tag
                            .AsNoTracking()
                            .Select(tag => new TagDTO
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            })
                            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TagDTO>> GetTagByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tag = await context
                        .Tag
                        .AsNoTracking()
                        .Select(tag => new TagDTO
                        {
                            Id = tag.Id,
                            Name = tag.Name
                        })
                        .FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);

        if (tag == null)
        {
            return Result<TagDTO>.Failure("Tag not found");
        }

        return Result<TagDTO>.Success(tag);
    }
}