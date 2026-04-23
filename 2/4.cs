Random rnd = new Random();

// Создаем двумерный массив со случайными числами
int rows = 5;
int cols = 4;
int[,] matrix = new int[rows, cols];

// Заполняем массив случайными числами от -50 до 50
Console.WriteLine("Сгенерированный массив:");
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        matrix[i, j] = rnd.Next(-50, 51);
        Console.Write(matrix[i, j] + "\t");
    }
    Console.WriteLine();
}

// Вводим номер строки
Console.Write("\nВведите номер строки (от 1 до " + rows + "): ");
int rowNumber = int.Parse(Console.ReadLine());
int rowIndex = rowNumber - 1;

// Вводим заданное число
Console.Write("Введите число для сравнения: ");
int givenNumber = int.Parse(Console.ReadLine());

// Считаем сумму элементов строки
int sum = 0;
for (int j = 0; j < cols; j++)
{
    sum += matrix[rowIndex, j];
}

// Проверяем условие
Console.WriteLine($"\nСумма {rowNumber}-й строки: {sum}");
Console.WriteLine($"Заданное число: {givenNumber}");

if (sum > givenNumber)
    Console.WriteLine($"ВЕРНО: Сумма строки {rowNumber} превышает число {givenNumber}");
else
    Console.WriteLine($"НЕ ВЕРНО: Сумма строки {rowNumber} НЕ превышает число {givenNumber}");