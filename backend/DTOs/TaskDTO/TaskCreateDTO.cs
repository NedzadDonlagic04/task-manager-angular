using System.ComponentModel.DataAnnotations;
using Validation;

namespace DTOs
{
    public class TaskCreateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(1_000, ErrorMessage = "Description must be between 0 and 1_000 characters.")]
        public string Description { get; set; } = null!;

        [MinimumFutureOffsetAttribute(1)]
        public DateTime? Deadline { get; set; }

        [Required]
        public List<Guid> TagIds { get; set; } = new();
    }
}
