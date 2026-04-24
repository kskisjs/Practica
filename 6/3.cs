using System;

// 1. Создаем пользовательский делегат
public delegate void StatusChangedEventHandler(string status);

// 2. Класс-издатель
public class OrderStatusManager
{
    // Событие на основе пользовательского делегата
    public event StatusChangedEventHandler StatusChanged;

    public void ChangeStatus(string newStatus)
    {
        Console.WriteLine($"\nМеняем статус на: {newStatus}");

        // Вызываем событие
        if (StatusChanged != null)
        {
            StatusChanged(newStatus);
        }
    }
}

// 3. Первый подписчик - оповещает клиента
public class CustomerNotifier
{
    public void OnStatusChanged(string status)
    {
        Console.WriteLine($"Клиент: Ваш заказ теперь имеет статус '{status}'");
    }
}

// 4. Второй подписчик - записывает в лог
public class AdminLogger
{
    public void OnStatusChanged(string status)
    {
        Console.WriteLine($"Лог: Статус заказа изменен на '{status}' в {DateTime.Now}");
    }
}

class Program
{
    static void Main()
    {
        // Создаем издателя
        OrderStatusManager manager = new OrderStatusManager();

        // Создаем подписчиков
        CustomerNotifier customer = new CustomerNotifier();
        AdminLogger admin = new AdminLogger();

        // Подписываем подписчиков на событие
        manager.StatusChanged += customer.OnStatusChanged;
        manager.StatusChanged += admin.OnStatusChanged;

        // Меняем статусы (событие срабатывает)
        manager.ChangeStatus("Принят");
        manager.ChangeStatus("В обработке");
        manager.ChangeStatus("Отправлен");
        manager.ChangeStatus("Доставлен");

        Console.ReadLine();
    }
}