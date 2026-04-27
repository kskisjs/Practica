using System;

// 1. Своя коллекция MyDictionary (простая версия)
public class MyDictionary<TKey, TValue>
{
    private TKey[] keys = new TKey[100];
    private TValue[] values = new TValue[100];
    private int count = 0;

    public void Add(TKey key, TValue value)
    {
        keys[count] = key;
        values[count] = value;
        count++;
    }

    public bool Remove(TKey key)
    {
        for (int i = 0; i < count; i++)
        {
            if (keys[i].Equals(key))
            {
                keys[i] = keys[count - 1];
                values[i] = values[count - 1];
                count--;
                return true;
            }
        }
        return false;
    }

    public TValue Find(TKey key)
    {
        for (int i = 0; i < count; i++)
            if (keys[i].Equals(key))
                return values[i];
        return default(TValue);
    }
}

// 2. Класс-контроллер
public class DictionaryManager<TKey, TValue>
{
    private MyDictionary<TKey, TValue> dict = new MyDictionary<TKey, TValue>();

    public void Add(TKey key, TValue value) => dict.Add(key, value);
    public void Remove(TKey key) => Console.WriteLine(dict.Remove(key) ? "Удалено" : "Не найдено");
    public void Find(TKey key)
    {
        TValue val = dict.Find(key);
        Console.WriteLine(val == null ? "Не найдено" : $"Найдено: {val}");
    }
}

// Использование
class Program
{
    static void Main()
    {
        var db = new DictionaryManager<int, string>();
        db.Add(1, "Иван");
        db.Add(2, "Мария");
        db.Find(1);
        db.Remove(1);
        db.Find(1);
    }
}