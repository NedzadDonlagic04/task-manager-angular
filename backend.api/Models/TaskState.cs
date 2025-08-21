using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class TaskState
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
