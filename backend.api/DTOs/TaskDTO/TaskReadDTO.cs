namespace DTOs
{
    public class TaskReadDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public DateTime Created_At { get; set; }
        public string TaskStateName { get; set; } = null!;
        public List<string> TagNames { get; set; } = new();
    }
}
