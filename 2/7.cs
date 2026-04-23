Console.Write("Введите строку: ");
string text = Console.ReadLine();

string result = text.Replace(" ", "");

Console.WriteLine($"Результат: {result}");