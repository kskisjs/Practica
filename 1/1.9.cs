class Task9
{
    static void Main()
    {
        double A = Math.PI / 4;   // π/4
        double B = Math.PI / 2;   // π/2
        int M = 15;

        double H = (B - A) / M;

        Console.WriteLine($"Табуляция функции y = 2 - sin(x)");
        Console.WriteLine($"Отрезок: [{A:F4}, {B:F4}], шаг: {H:F4}\n");
        Console.WriteLine("    x        y");
        Console.WriteLine("----------------");

        for (int i = 0; i <= M; i++)
        {
            double x = A + i * H;
            double y = 2 - Math.Sin(x);
            Console.WriteLine($"{x:F4}    {y:F4}");
        }
    }
}