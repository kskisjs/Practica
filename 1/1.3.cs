class Task3
{
    static void Main()
    {
        Console.Write("Введите A: ");
        int A = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введите B: ");
        int B = Convert.ToInt32(Console.ReadLine());

        int count = 0;

        for (int i = A; i <= B; i++)
        {
            Console.Write(i + " ");
            count++;
        }

        Console.WriteLine($"\nКоличество чисел: {count}");
    }
}
