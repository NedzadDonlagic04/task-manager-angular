namespace DTOs;

public sealed record TaskStateDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}