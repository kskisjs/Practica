using System;
using System.Collections.Generic;

// 1. Обобщенный интерфейс
public interface IQueue<T>
{
    void Enqueue(T item);
    T Dequeue();
    T Peek();
}

// 2. Реализация интерфейса
public class SimpleQueue<T> : IQueue<T>
{
    private Queue<T> queue = new Queue<T>();

    public void Enqueue(T item)
    {
        queue.Enqueue(item);
    }

    public T Dequeue()
    {
        return queue.Dequeue();
    }

    public T Peek()
    {
        return queue.Peek();
    }

    public bool IsEmpty()
    {
        return queue.Count == 0;
    }
}

// 3. Класс-хранилище (репозиторий)
public class QueueRepository<T>
{
    private IQueue<T> queue;

    public QueueRepository(IQueue<T> queue)
    {
        this.queue = queue;
    }

    public void Add(T item) => queue.Enqueue(item);
    public T Remove() => queue.Dequeue();
    public T ViewFirst() => queue.Peek();
    public IQueue<T> GetQueue() => queue;
}

// 4. Класс бизнес-логики
public class QueueManager<T>
{
    private QueueRepository<T> repo;

    public QueueManager(IQueue<T> queue)
    {
        repo = new QueueRepository<T>(queue);
    }

    public void AddItem(T item)
    {
        repo.Add(item);
        Console.WriteLine($"Добавлено: {item}");
    }

    public void RemoveItem()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Очередь пуста");
            return;
        }
        Console.WriteLine($"Удалено: {repo.Remove()}");
    }

    public void PeekItem()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Очередь пуста");
            return;
        }
        Console.WriteLine($"Первый элемент: {repo.ViewFirst()}");
    }

    public void PrintQueue()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Очередь пуста");
            return;
        }

        Console.Write("Содержимое очереди: ");
        var temp = new Queue<T>();
        var queue = repo.GetQueue();

        while (!((SimpleQueue<T>)queue).IsEmpty())
        {
            var item = queue.Dequeue();
            Console.Write($"{item} ");
            temp.Enqueue(item);
        }

        while (temp.Count > 0)
            queue.Enqueue(temp.Dequeue());

        Console.WriteLine();
    }

    public bool IsEmpty()
    {
        return ((SimpleQueue<T>)repo.GetQueue()).IsEmpty();
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        var queue = new SimpleQueue<string>();
        var manager = new QueueManager<string>(queue);

        manager.AddItem("Первый");
        manager.AddItem("Второй");
        manager.AddItem("Третий");

        manager.PrintQueue();
        manager.PeekItem();
        manager.RemoveItem();
        manager.PrintQueue();
        manager.RemoveItem();
        manager.RemoveItem();
        manager.RemoveItem(); // Очередь пуста
    }
}