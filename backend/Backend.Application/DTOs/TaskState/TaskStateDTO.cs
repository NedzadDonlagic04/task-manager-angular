namespace Backend.Application.DTOs.TaskState;

public sealed record TaskStateDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
