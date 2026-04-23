
class Program
{
    static void Main()
    {
        const double g = 9.81523;  // ускорение свободного падения, м/с^2

        Console.Write("Введите высоту h (в метрах): ");
        double h = Convert.ToDouble(Console.ReadLine());

        if (h < 0)
        {
            Console.WriteLine("Высота не может быть отрицательной!");
        }
        else
        {
            double t = Math.Sqrt(2 * h / g);
            Console.WriteLine($"\nВысота: {h} м");
            Console.WriteLine($"Время падения: {t:F4} секунд");
        }
    }
}