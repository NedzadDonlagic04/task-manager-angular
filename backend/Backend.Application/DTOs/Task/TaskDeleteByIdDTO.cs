namespace Backend.Application.DTOs.Task;

public sealed class TaskDeleteByIdDTO
{
    public required Guid OwnerId { get; init; }
    public required Guid TaskId { get; init; }
}
