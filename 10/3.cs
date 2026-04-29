using System;
using System.Collections.Generic;

// Интерфейс подписчика
public interface IStockObserver
{
    void Update(string stockName, double price);
}

// Издатель (StockMarket)
public class StockMarket
{
    private Dictionary<string, double> _stocks = new Dictionary<string, double>();
    private Dictionary<string, List<IStockObserver>> _observers = new Dictionary<string, List<IStockObserver>>();

    // Добавить акцию
    public void AddStock(string name, double initialPrice)
    {
        _stocks[name] = initialPrice;
        _observers[name] = new List<IStockObserver>();
        Console.WriteLine($"Акция {name} добавлена. Цена: {initialPrice}");
    }

    // Подписка на акцию
    public void Subscribe(string stockName, IStockObserver observer)
    {
        if (_observers.ContainsKey(stockName))
        {
            _observers[stockName].Add(observer);
            Console.WriteLine($"{observer.GetType().Name} подписался на {stockName}");
        }
    }

    // Отписка от акции
    public void Unsubscribe(string stockName, IStockObserver observer)
    {
        if (_observers.ContainsKey(stockName))
        {
            _observers[stockName].Remove(observer);
        }
    }

    // Изменение цены акции
    public void SetPrice(string stockName, double newPrice)
    {
        if (_stocks.ContainsKey(stockName))
        {
            double oldPrice = _stocks[stockName];
            _stocks[stockName] = newPrice;
            Console.WriteLine($"\nЦена {stockName} изменилась: {oldPrice} -> {newPrice}");
            NotifyObservers(stockName, newPrice);
        }
    }

    // Уведомление подписчиков
    private void NotifyObservers(string stockName, double price)
    {
        foreach (var observer in _observers[stockName])
        {
            observer.Update(stockName, price);
        }
    }
}

// Конкретный подписчик (Инвестор)
public class Investor : IStockObserver
{
    private string _name;

    public Investor(string name)
    {
        _name = name;
    }

    public void Update(string stockName, double price)
    {
        Console.WriteLine($"Инвестор {_name}: Акция {stockName} теперь стоит {price}");
    }
}

// Демонстрация
class Program
{
    static void Main()
    {
        StockMarket market = new StockMarket();

        // Добавляем акции
        market.AddStock("Apple", 150.0);
        market.AddStock("Google", 2800.0);

        // Создаём инвесторов
        Investor investor1 = new Investor("Иван");
        Investor investor2 = new Investor("Мария");
        Investor investor3 = new Investor("Петр");

        // Подписка на акции
        market.Subscribe("Apple", investor1);
        market.Subscribe("Apple", investor2);
        market.Subscribe("Google", investor2);
        market.Subscribe("Google", investor3);

        // Изменяем цены
        market.SetPrice("Apple", 155.5);
        market.SetPrice("Google", 2750.0);
        market.SetPrice("Apple", 160.0);
    }
}