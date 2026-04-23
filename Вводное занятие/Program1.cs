using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Вычисление объема цилиндра");
        Console.WriteLine("Введите исходные данные:");

        Console.Write("Радиус основания (см) ---> ");
        double radius = double.Parse(Console.ReadLine());

        Console.Write("Высота цилиндра (см) ---> ");
        double height = double.Parse(Console.ReadLine());

        double volume = Math.PI * Math.Pow(radius, 2) * height;

        Console.WriteLine($"Объем цилиндра: {volume:F2} куб. см.");
    }
}