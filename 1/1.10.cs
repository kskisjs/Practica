class AutomorphicNumbers
{
    static void Main()
    {
        Console.WriteLine("Трёхзначные автоморфные числа:");
        Console.WriteLine("(Число равно последним цифрам своего квадрата)\n");

        int count = 0;

        for (int num = 100; num <= 999; num++)
        {
            long square = (long)num * num;  // квадрат числа

            // Проверяем, оканчивается ли квадрат на наше число
            if (square % 1000 == num)
            {
                Console.WriteLine($"{num}^2 = {square} → {num} - автоморфное число");
                count++;
            }
        }

        if (count == 0)
        {
            Console.WriteLine("Трёхзначных автоморфных чисел не найдено.");
        }
        else
        {
            Console.WriteLine($"\nНайдено чисел: {count}");
        }
    }
}