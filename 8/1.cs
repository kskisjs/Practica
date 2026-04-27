using System;
using System.Collections;

// Модельный класс
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Priority { get; set; }
    
    public Task(int id, string title, string priority)
    {
        Id = id;
        Title = title;
        Priority = priority;
    }
    
    public override string ToString()
    {
        return $"#{Id}: {Title} [{Priority}]";
    }
}

// Класс-менеджер
public class TaskManager
{
    private Queue queue = new Queue();
    private int nextId = 1;
    
    public void AddTask(string title, string priority)
    {
        queue.Enqueue(new Task(nextId++, title, priority));
        Console.WriteLine($"Добавлена: {title}");
    }
    
    public void ProcessTask()
    {
        if (queue.Count > 0)
            Console.WriteLine($"Обработана: {queue.Dequeue()}");
        else
            Console.WriteLine("Нет задач");
    }
    
    public void GetPendingTasks()
    {
        Console.WriteLine($"\nОжидающих задач: {queue.Count}");
        foreach (Task task in queue)
            Console.WriteLine($"  {task}");
    }
}

// Использование
class Program
{
    static void Main()
    {
        TaskManager tm = new TaskManager();
        
        tm.AddTask("Исправить баг", "High");
        tm.AddTask("Написать тесты", "Medium");
        tm.AddTask("Обновить доки", "Low");
        
        tm.GetPendingTasks();
        
        tm.ProcessTask();
        tm.ProcessTask();
        
        tm.GetPendingTasks();
    }
}