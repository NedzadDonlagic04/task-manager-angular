namespace Backend.Application.DTOs.Task;

public sealed record TaskGetByIdDTO
{
    public required Guid TaskId { get; init; }
    public required Guid OwnerId { get; init; }
}
