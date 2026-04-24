using System;

// Класс для передачи данных события
public class TemperatureEventArgs : EventArgs
{
    public double Temperature { get; set; }
    public string RoomName { get; set; }

    public TemperatureEventArgs(double temperature, string roomName)
    {
        Temperature = temperature;
        RoomName = roomName;
    }
}

// 1. Класс-издатель с событием на основе EventHandler
public class TemperatureSensor
{
    // Событие на основе EventHandler<T>
    public event EventHandler<TemperatureEventArgs> TemperatureChanged;

    private double _currentTemperature;

    public void SetTemperature(double newTemperature, string roomName)
    {
        _currentTemperature = newTemperature;

        Console.WriteLine($"\n[Датчик] Температура в {roomName}: {newTemperature}°C");

        // Вызываем событие
        TemperatureChanged?.Invoke(this, new TemperatureEventArgs(newTemperature, roomName));
    }
}

// 2. Первый подписчик - система охлаждения (кондиционер)
public class CoolingSystem
{
    public void OnTemperatureChanged(object sender, TemperatureEventArgs e)
    {
        if (e.Temperature > 25)
        {
            Console.WriteLine($"[Кондиционер] В {e.RoomName} жарко ({e.Temperature}°C)! Включаем охлаждение");
        }
        else if (e.Temperature < 18)
        {
            Console.WriteLine($"[Кондиционер] В {e.RoomName} холодно ({e.Temperature}°C)! Выключаем охлаждение");
        }
        else
        {
            Console.WriteLine($"[Кондиционер] В {e.RoomName} нормальная температура ({e.Temperature}°C). Режим ожидания");
        }
    }
}

// 3. Второй подписчик - система оповещения
public class AlarmSystem
{
    public void OnTemperatureChanged(object sender, TemperatureEventArgs e)
    {
        if (e.Temperature >= 30)
        {
            Console.WriteLine($"[СИГНАЛИЗАЦИЯ] ВНИМАНИЕ! В {e.RoomName} критическая температура {e.Temperature}°C! Возможен перегрев!");
        }
        else if (e.Temperature <= 10)
        {
            Console.WriteLine($"[СИГНАЛИЗАЦИЯ] ВНИМАНИЕ! В {e.RoomName} слишком низкая температура {e.Temperature}°C!");
        }
        else
        {
            Console.WriteLine($"[Оповещение] Температура в {e.RoomName} в норме ({e.Temperature}°C)");
        }
    }
}

// 4. Промежуточный класс, который подписывает подписчиков
public class TemperatureMonitor
{
    public TemperatureMonitor(TemperatureSensor sensor, CoolingSystem cooling, AlarmSystem alarm)
    {
        // Подписываем подписчиков на событие датчика
        sensor.TemperatureChanged += cooling.OnTemperatureChanged;
        sensor.TemperatureChanged += alarm.OnTemperatureChanged;

        Console.WriteLine("[Монитор] Система контроля температуры активирована");
        Console.WriteLine("[Монитор] Кондиционер и сигнализация подключены к датчику\n");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Система контроля температуры (EventHandler) ===\n");

        // Создаем издателя
        TemperatureSensor sensor = new TemperatureSensor();

        // Создаем подписчиков
        CoolingSystem cooling = new CoolingSystem();
        AlarmSystem alarm = new AlarmSystem();

        // Создаем промежуточный класс, который подписывает подписчиков
        TemperatureMonitor monitor = new TemperatureMonitor(sensor, cooling, alarm);

        // Изменяем температуру для проверки работы системы
        Console.WriteLine("=== Проверка работы системы ===\n");

        sensor.SetTemperature(22, "Гостиная");
        sensor.SetTemperature(27, "Спальня");
        sensor.SetTemperature(16, "Кухня");
        sensor.SetTemperature(32, "Серверная");
        sensor.SetTemperature(8, "Склад");
        sensor.SetTemperature(24, "Кабинет");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}