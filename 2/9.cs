using System.Text;

StringBuilder sb = new StringBuilder("Привет");

Console.WriteLine($"Было: {sb}");

sb.Append(" мир!");

Console.WriteLine($"Стало: {sb}");
