using System;
using System.Collections.Generic;

// Интерфейс стратегии
public interface IFilterStrategy
{
    bool Filter(int number);
}

// Чётные числа
public class EvenNumberFilter : IFilterStrategy
{
    public bool Filter(int number) => number % 2 == 0;
}

// Простые числа
public class PrimeNumberFilter : IFilterStrategy
{
    public bool Filter(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i < number; i++)
            if (number % i == 0) return false;
        return true;
    }
}

// Диапазон
public class RangeFilter : IFilterStrategy
{
    private int _min, _max;
    public RangeFilter(int min, int max) { _min = min; _max = max; }
    public bool Filter(int number) => number >= _min && number <= _max;
}

// Контекст
public class DataFilter
{
    private IFilterStrategy _strategy;

    public void SetStrategy(IFilterStrategy strategy)
    {
        _strategy = strategy;
    }

    public List<int> Filter(int[] data)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < data.Length; i++)
        {
            if (_strategy.Filter(data[i]))
                result.Add(data[i]);
        }
        return result;
    }
}

// Демонстрация
class Program
{
    static void Main()
    {
        int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        DataFilter filter = new DataFilter();

        filter.SetStrategy(new EvenNumberFilter());
        Console.WriteLine("Чётные: " + string.Join(", ", filter.Filter(data)));

        filter.SetStrategy(new PrimeNumberFilter());
        Console.WriteLine("Простые: " + string.Join(", ", filter.Filter(data)));

        filter.SetStrategy(new RangeFilter(5, 10));
        Console.WriteLine("От 5 до 10: " + string.Join(", ", filter.Filter(data)));
    }
}