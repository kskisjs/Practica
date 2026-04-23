int[] numbers = new int[50];
Random rnd = new Random();

// Заполняем массив
for (int i = 0; i < 50; i++)
    numbers[i] = rnd.Next(1, 100);

// Выводим массив
Console.WriteLine("Массив:");
foreach (int n in numbers)
    Console.Write(n + " ");

// Считаем числа, отличные от последнего
int last = numbers[49];
int count = 0;
for (int i = 0; i < 49; i++)
    if (numbers[i] != last) count++;

Console.WriteLine($"\n\nПоследнее число: {last}");
Console.WriteLine($"Чисел, отличных от последнего: {count}");

// Сортируем
Array.Sort(numbers);
Console.WriteLine("\nОтсортированный массив:");
foreach (int n in numbers)
    Console.Write(n + " ");

// Бинарный поиск
Console.Write("\n\nВведите число для поиска: ");
int k = int.Parse(Console.ReadLine());

int pos = Array.BinarySearch(numbers, k);
if (pos >= 0)
    Console.WriteLine($"Число {k} найдено!");
else
    Console.WriteLine($"Число {k} не найдено");