using DTOs;
using Utils;

namespace Services {
    public interface ITagService {
        Task<IEnumerable<TagDTO>> GetTagsAsync();
        Task<Result<TagDTO>> GetTagByIdAsync(Guid id);
    }
}
