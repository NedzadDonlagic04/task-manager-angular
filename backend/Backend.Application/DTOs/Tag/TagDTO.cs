namespace Backend.Application.DTOs.Tag;

public sealed record TagDTO
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
