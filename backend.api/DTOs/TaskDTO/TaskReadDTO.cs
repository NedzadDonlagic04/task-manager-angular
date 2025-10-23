namespace DTOs;

public record TaskReadDTO
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateTime? Deadline { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string TaskStateName { get; init; }
    public required IReadOnlyList<string> TagNames { get; init; }
}
