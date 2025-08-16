namespace DTOs
{
    public class TaskCreateDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public List<Guid> TagIds { get; set; } = new();
    }
}
