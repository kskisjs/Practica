namespace TodoListApp.Services
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
