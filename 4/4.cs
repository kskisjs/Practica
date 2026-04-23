using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите число: ");
        int num = int.Parse(Console.ReadLine());

        Console.WriteLine($"{num} четное? {num.IsEven()}");
    }
}

static class IntExtensions
{
    public static bool IsEven(this int number)
    {
        return number % 2 == 0;
    }
}