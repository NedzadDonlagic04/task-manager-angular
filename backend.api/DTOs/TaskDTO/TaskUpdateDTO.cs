using System.ComponentModel.DataAnnotations;
using Enums;
using Validation;

namespace DTOs
{
    public class TaskUpdateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(1_000, ErrorMessage = "Description must be between 0 and 1_000 characters.")]
        public string Description { get; set; } = null!;

        [MinimumFutureOffsetAttribute(1)]
        public DateTime? Deadline { get; set; }

        [Required]
        [EnumValidationAttribute(typeof(TaskStateEnum))]
        public int TaskStateId { get; set; }

        [Required]
        public List<Guid> TagIds { get; set; } = new();
    }
}
