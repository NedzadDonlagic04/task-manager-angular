using Backend.Application.DTOs;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Tasks;

public interface ITagService
{
    Task<IEnumerable<TagDTO>> GetTagsAsync(CancellationToken cancellationToken = default);
    Task<Result<TagDTO>> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
