namespace Backend.Application.DTOs.Task;

public sealed record TaskBulkDeleteDTO
{
    public required Guid OwnerId { get; init; }
    public required List<Guid> TaskIds { get; init; }
}
