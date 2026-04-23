Random rnd = new Random();
double[] numbers = new double[15];

// Заполняем массив случайными вещественными числами от -50 до 50
for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = Math.Round(rnd.NextDouble() * 100 - 50, 2);
}

double sum = 0;
int count = 0;

Console.WriteLine("Сгенерированный массив:");
foreach (double num in numbers)
{
    Console.Write(num + " ");
    sum += num;
    count++;
}

Console.WriteLine($"\n\nСумма элементов: {Math.Round(sum, 2)}");
Console.WriteLine($"Количество элементов: {count}");