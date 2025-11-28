using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Backend.Domain.Entities.Tasks;
using Backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services;

public sealed class TagService(IAppDbContext context) : ITagService
{
    public async Task<IEnumerable<TagDTO>> GetTagsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var results = await context
            .Set<TagEntity>()
            .AsNoTracking()
            .Select(tag => new TagDTO { Id = tag.Id, Name = tag.Name })
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TagDTO>> GetTagByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var tag = await context
            .Set<TagEntity>()
            .AsNoTracking()
            .Select(tag => new TagDTO { Id = tag.Id, Name = tag.Name })
            .FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);

        return tag is null ? Result<TagDTO>.Failure("Tag not found") : Result<TagDTO>.Success(tag);
    }
}
