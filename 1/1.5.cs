class Task5
{
    static void Main()
    {
        Console.Write("Введите целое число: ");
        int num = Convert.ToInt32(Console.ReadLine());

        if (num % 2 == 0)
        {
            Console.WriteLine($"Число {num} является четным");
        }
        else
        {
            Console.WriteLine($"Число {num} является нечетным");
        }
    }
}