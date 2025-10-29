using System.ComponentModel.DataAnnotations;
using Backend.Application.Validation;

namespace Backend.Application.DTOs.TaskDTO;

public sealed record TaskCreateUpdateDTO
{
    [Required]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 50 characters."
    )]
    public required string Title { get; init; }

    [StringLength(1_000, ErrorMessage = "Description must be between 0 and 1_000 characters.")]
    public required string Description { get; init; }

    [MinimumFutureOffset(1)]
    public DateTimeOffset? Deadline { get; init; }

    [Required]
    public required IReadOnlyList<Guid> TagIds { get; init; }
}
