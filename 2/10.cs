using System.Text.RegularExpressions;

Console.Write("Введите email: ");
string email = Console.ReadLine();

//регулярное выражение
string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

bool isValid = Regex.IsMatch(email, pattern);

Console.WriteLine(isValid ? "Email корректный" : "Email НЕ корректный");