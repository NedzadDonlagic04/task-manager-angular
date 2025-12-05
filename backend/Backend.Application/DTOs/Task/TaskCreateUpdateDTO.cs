using System.ComponentModel.DataAnnotations;
using Backend.Application.Attributes;

namespace Backend.Application.DTOs.Task;

public sealed record TaskCreateUpdateDTO
{
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "{0} must be between {2} and {1} characters"
    )]
    public required string Title { get; init; }

    [StringLength(1_000, ErrorMessage = "{0} cannot exceed {1} characters")]
    public required string Description { get; init; }

    [MinimumFutureOffset(1)]
    public DateTimeOffset? Deadline { get; init; }

    public required IReadOnlyList<Guid> TagIds { get; init; }
}
