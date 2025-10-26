using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities;

public sealed class TaskState
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
