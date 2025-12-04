namespace Backend.Application.DTOs.Task;

public sealed record TaskReadDTO
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateTimeOffset? Deadline { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required string TaskStateName { get; init; }
    public required IReadOnlyList<string> TagNames { get; init; }
}
