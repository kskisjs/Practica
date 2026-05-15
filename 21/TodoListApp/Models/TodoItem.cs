using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        public bool IsCompleted { get; set; }

        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
    }
}