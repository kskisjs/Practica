namespace TodoListApp.Services
{
    public interface ITaskService
    {
        void AddTask(TaskDto task);
        void CompleteTask(int id);
        List<TaskDto> GetTasks();
        TaskDto? GetById(int id);
    }
}
