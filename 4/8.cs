using System;

class Program
{
    static void Main()
    {
        // Пример с Цельсием
        double result;
        ConvertCelsiusToFahrenheit(25, out result);
        Console.WriteLine($"25°C = {result}°F");

        // Пример с Кельвином
        ConvertKelvinToFahrenheit(298.15, out result);
        Console.WriteLine($"298.15K = {result}°F");

        // Ввод с клавиатуры
        Console.WriteLine("\n=== Ввод с клавиатуры ===");
        Console.Write("Выберите шкалу (1 - Цельсий, 2 - Кельвин): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.Write("Введите температуру в Цельсиях: ");
            double celsius = double.Parse(Console.ReadLine());
            ConvertCelsiusToFahrenheit(celsius, out result);
            Console.WriteLine($"{celsius}°C = {result}°F");
        }
        else if (choice == "2")
        {
            Console.Write("Введите температуру в Кельвинах: ");
            double kelvin = double.Parse(Console.ReadLine());
            ConvertKelvinToFahrenheit(kelvin, out result);
            Console.WriteLine($"{kelvin}K = {result}°F");
        }
        else
        {
            Console.WriteLine("Неверный выбор");
        }
    }

    static void ConvertCelsiusToFahrenheit(in double celsius, out double fahrenheit)
    {
        // °F = (°C × 9/5) + 32
        fahrenheit = (celsius * 9 / 5) + 32;
    }

    static void ConvertKelvinToFahrenheit(in double kelvin, out double fahrenheit)
    {
        // K → °C → °F: °F = ((K - 273.15) × 9/5) + 32
        fahrenheit = ((kelvin - 273.15) * 9 / 5) + 32;
    }
}