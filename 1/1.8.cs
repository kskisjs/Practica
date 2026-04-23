class Task8
{
    static void Main()
    {
        Console.Write("Введите N (1-20): ");
        int N = Convert.ToInt32(Console.ReadLine());

        double sum = 0;
        double term = 1.1;

        for (int i = 1; i <= N; i++)
        {
            if (i % 2 == 1)  // нечетные - плюс
            {
                sum += term;
            }
            else  // четные - минус
            {
                sum -= term;
            }
            term += 0.1;
        }

        Console.WriteLine($"Сумма = {sum:F4}");
    }
}