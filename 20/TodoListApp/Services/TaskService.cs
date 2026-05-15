namespace TodoListApp.Services
{
    public class TaskService : ITaskService
    {
        private static List<TaskDto> _tasks = new()
        {
            new TaskDto { Id = 1, Description = "Купить продукты",  DueDate = DateTime.Today.AddDays(1),  IsCompleted = false },
            new TaskDto { Id = 2, Description = "Прочитать книгу",  DueDate = DateTime.Today.AddDays(-2), IsCompleted = true  },
            new TaskDto { Id = 3, Description = "Сделать зарядку",  DueDate = DateTime.Today,             IsCompleted = false },
        };
        private static int _nextId = 4;

        public void AddTask(TaskDto task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }

        public void CompleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
                task.IsCompleted = true;
        }

        public List<TaskDto> GetTasks() => _tasks.ToList();

        public TaskDto? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);
    }
}
