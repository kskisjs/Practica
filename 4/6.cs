using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите часы (0-23): ");
        int H = int.Parse(Console.ReadLine());

        Console.Write("Введите минуты (0-59): ");
        int M = int.Parse(Console.ReadLine());

        Console.Write("Введите секунды (0-59): ");
        int S = int.Parse(Console.ReadLine());

        Console.Write("Введите количество секунд для увеличения: ");
        int T = int.Parse(Console.ReadLine());

        Console.WriteLine($"Исходное время: {H}:{M}:{S}");

        IncTime(ref H, ref M, ref S, T);

        Console.WriteLine($"Время после увеличения на {T} секунд: {H}:{M}:{S}");
    }

    static void IncTime(ref int H, ref int M, ref int S, int T)
    {
        S += T;

        M += S / 60;
        S = S % 60;

        H += M / 60;
        M = M % 60;

        H = H % 24;
    }
}