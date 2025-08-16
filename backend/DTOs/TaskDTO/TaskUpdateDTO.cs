namespace DTOs
{
    public class TaskUpdateDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public int TaskStateId { get; set; }
        public List<Guid> TagIds { get; set; } = new();
    }
}
