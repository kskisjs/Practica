using System;

class Program
{
    static void Main()
    {
        Order[] orders = new Order[]
        {
            new Order(1, "Иван Петров", 15000),
            new Order(2, "Мария Иванова", 5000),
            new Order(3, "Иван Петров", 25000),
            new Order(4, "Алексей Сидоров", 8000),
            new Order(5, "Мария Иванова", 12000)
        };

        Store store = new Store(orders);

        Console.WriteLine("=== ВСЕ ЗАКАЗЫ ===");
        foreach (Order o in orders) o.Print();

        Console.WriteLine("\n=== ЗАКАЗЫ ДОРОЖЕ 10000 РУБ. ===");
        foreach (Order o in store.GetHighValueOrders(10000)) o.Print();

        Console.WriteLine("\n=== ЗАКАЗЫ ИВАНА ПЕТРОВА ===");
        foreach (Order o in store.GetOrdersByCustomer("Иван Петров")) o.Print();

        Console.ReadLine();
    }
}