using System;

// 1. Создаем делегат MathOperation, который принимает два числа и возвращает результат
public delegate double MathOperation(double a, double b);

class Program
{
    // 2. Метод Calculate принимает два числа и делегат (callback)
    static double Calculate(double x, double y, MathOperation operation)
    {
        // Вызываем переданный делегат и возвращаем результат
        return operation(x, y);
    }

    // 3. Метод сложения
    static double Add(double a, double b)
    {
        return a + b;
    }

    // 4. Метод умножения
    static double Multiply(double a, double b)
    {
        return a * b;
    }

    static void Main()
    {
        Console.WriteLine("=== Передача делегата в качестве параметра метода ===\n");

        double num1 = 10;
        double num2 = 5;

        // Передаем метод Add в качестве callback
        double sumResult = Calculate(num1, num2, Add);
        Console.WriteLine($"Вызов Calculate с методом Add: {num1} + {num2} = {sumResult}");

        // Передаем метод Multiply в качестве callback
        double multiplyResult = Calculate(num1, num2, Multiply);
        Console.WriteLine($"Вызов Calculate с методом Multiply: {num1} × {num2} = {multiplyResult}");

        // Дополнительная демонстрация с другими числами
        Console.WriteLine("\n=== Демонстрация с другими числами ===");

        double a = 7.5;
        double b = 2.5;

        Console.WriteLine($"Числа: {a} и {b}");
        Console.WriteLine($"Сложение через Calculate: {Calculate(a, b, Add)}");
        Console.WriteLine($"Умножение через Calculate: {Calculate(a, b, Multiply)}");
    }
}