Console.Write("Введите размер матрицы N (N<10): ");
int N = int.Parse(Console.ReadLine());

Console.Write("Введите минимальное значение a: ");
int a = int.Parse(Console.ReadLine());

Console.Write("Введите максимальное значение b: ");
int b = int.Parse(Console.ReadLine());

int[,] matrix = new int[N, N];
Random rnd = new Random();

// Заполняем матрицу случайными числами
Console.WriteLine("\nИсходная матрица:");
for (int i = 0; i < N; i++)
{
    for (int j = 0; j < N; j++)
    {
        matrix[i, j] = rnd.Next(a, b + 1);
        Console.Write(matrix[i, j] + "\t");
    }
    Console.WriteLine();
}

// 1. Сумма отрицательных элементов
int sumNegative = 0;
for (int i = 0; i < N; i++)
{
    for (int j = 0; j < N; j++)
    {
        if (matrix[i, j] < 0)
            sumNegative += matrix[i, j];
    }
}
Console.WriteLine($"\nСумма отрицательных элементов: {sumNegative}");

// 2. Количество чётных элементов в каждой строке
Console.WriteLine("\nКоличество чётных элементов в каждой строке:");
for (int i = 0; i < N; i++)
{
    int evenCount = 0;
    for (int j = 0; j < N; j++)
    {
        if (matrix[i, j] % 2 == 0)
            evenCount++;
    }
    Console.WriteLine($"Строка {i + 1}: {evenCount} чётных элементов");
}