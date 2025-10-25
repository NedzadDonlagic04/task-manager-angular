namespace Backend.Domain.Models;

public sealed class TaskTag
{
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
