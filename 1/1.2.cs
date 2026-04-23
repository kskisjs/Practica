class Task2
{
    static void Main()
    {
        Console.Write("Введите x: ");
        double x = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите y: ");
        double y = Convert.ToDouble(Console.ReadLine());

        bool isSecondQuadrant = (x < 0 && y > 0);

        Console.WriteLine($"Точка лежит во второй четверти: {isSecondQuadrant}");
    }
}