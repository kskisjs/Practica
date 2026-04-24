using System;

// 1. Создаем делегат MathOperation, который принимает два числа и возвращает результат
public delegate double MathOperation(double a, double b);

// 2. Первый класс с методом сложения
public class Addition
{
    public double Add(double a, double b)
    {
        return a + b;
    }
}

// 3. Второй класс с методом умножения
public class Multiplication
{
    public double Multiply(double a, double b)
    {
        return a * b;
    }
}

class Program
{
    static void Main()
    {
        // Создаем экземпляры классов
        Addition additionObj = new Addition();
        Multiplication multiplicationObj = new Multiplication();

        // Передаем методы в делегат
        MathOperation operation1 = additionObj.Add;
        MathOperation operation2 = multiplicationObj.Multiply;

        // Вызываем делегат
        double result1 = operation1(10, 5);
        double result2 = operation2(10, 5);

        // Выводим результаты
        Console.WriteLine($"Результат сложения: {result1}");
        Console.WriteLine($"Результат умножения: {result2}");

        // Дополнительная демонстрация работы с разными числами
        double x = 7.5, y = 2.5;

        Console.WriteLine($"\nДемонстрация с числами {x} и {y}:");
        Console.WriteLine($"Сложение: {operation1(x, y)}");
        Console.WriteLine($"Умножение: {operation2(x, y)}");
    }
}