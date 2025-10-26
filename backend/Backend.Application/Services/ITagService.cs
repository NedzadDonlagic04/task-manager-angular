using Backend.Application.DTOs;
using Backend.Shared.Utils;

namespace Backend.Application.Services;

public interface ITagService
{
    Task<IEnumerable<TagDTO>> GetTagsAsync(CancellationToken cancellationToken);
    Task<Result<TagDTO>> GetTagByIdAsync(Guid id, CancellationToken cancellationToken);
}
