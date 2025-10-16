namespace DTOs
{
    public record TagDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
