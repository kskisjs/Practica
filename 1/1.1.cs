class Task1
{
    static void Main()
    {
        Console.Write("Введите массу в килограммах: ");
        double kg = Convert.ToDouble(Console.ReadLine());

        int centners = (int)(kg / 100);

        Console.WriteLine($"Число полных центнеров: {centners}");
    }
}