using DTOs;

namespace Services {
    public interface ITagService {
        Task<IEnumerable<TagDTO>> GetTagsAsync();
        Task<TagDTO?> GetTagByIdAsync(Guid id);
    }
}
