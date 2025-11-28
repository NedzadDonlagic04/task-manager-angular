using Backend.Application.DTOs;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagDTO>> GetTagsAsync(CancellationToken cancellationToken);
    Task<Result<TagDTO>> GetTagByIdAsync(Guid id, CancellationToken cancellationToken);
}
