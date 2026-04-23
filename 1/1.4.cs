class Task4
{
    static void Main()
    {
        Console.Write("Введите x (в радианах): ");
        double x = Convert.ToDouble(Console.ReadLine());

        double y = 2 - Math.Sin(x);

        Console.WriteLine($"y = 2 - sin({x}) = {y:F4}");
    }
}