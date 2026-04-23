class Program
{
    static void Main(string[] args)
    {
        Random rand = new Random();
        int[] numbers = new int[10];

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rand.Next(1, 100);
        }

        Console.Write("Массив: ");
        for (int i = 0; i < numbers.Length; i++)
        {
            Console.Write(numbers[i] + " ");
        }

        Console.WriteLine();

        int result = ArraySumCalculator.SumOfArray(numbers);

        Console.WriteLine("Сумма элементов массива: " + result);
    }
}

static class ArraySumCalculator
{
    public static int SumOfArray(int[] array)
    {
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }
}