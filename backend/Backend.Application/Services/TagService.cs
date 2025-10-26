using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Backend.Domain.Models;
using Backend.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services;

public sealed class TagService(IAppDbContext context) : ITagService
{
    public async Task<IEnumerable<TagDTO>> GetTagsAsync(CancellationToken cancellationToken)
    {
        var results = await context
            .Set<Tag>()
            .AsNoTracking()
            .Select(tag => new TagDTO { Id = tag.Id, Name = tag.Name })
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Result<TagDTO>> GetTagByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tag = await context
            .Set<Tag>()
            .AsNoTracking()
            .Select(tag => new TagDTO { Id = tag.Id, Name = tag.Name })
            .FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);

        return tag == null ? Result<TagDTO>.Failure("Tag not found") : Result<TagDTO>.Success(tag);
    }
}
