
class Program
{
    static void Main()
    {
        Console.WriteLine("Расчет по двум формулам (Вариант 2)");
        Console.Write("Введите значение угла α (в градусах): ");

        double alphaDeg = double.Parse(Console.ReadLine());
        double alphaRad = alphaDeg * Math.PI / 180;

        double sinA = Math.Sin(alphaRad);
        double cosA = Math.Cos(alphaRad);

        // Первая формула
        double z1 = (1 - sinA * sinA) / (1 + Math.Sin(2 * alphaRad));

        // Вторая формула
        double tg = sinA / cosA;
        double z2 = (1 - tg) / (1 + tg);

        Console.WriteLine($"\nz1 = {z1:F6}");
        Console.WriteLine($"z2 = {z2:F6}");
    }
}