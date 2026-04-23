Console.Write("Введите строку: ");
string text = Console.ReadLine();

// Разбиваем строку на слова
string[] words = text.Split(' ');

string mostFrequent = "";
int maxCount = 0;

for (int i = 0; i < words.Length; i++)
{
    int count = 0;
    for (int j = 0; j < words.Length; j++)
    {
        if (words[i] == words[j])
            count++;
    }

    if (count > maxCount)
    {
        maxCount = count;
        mostFrequent = words[i];
    }
}

Console.WriteLine($"Самое частое слово: {mostFrequent}");
Console.WriteLine($"Встречается {maxCount} раз(а)");
