using System;
using System.IO;

namespace JsonParsingTask
{
    // Пользовательское исключение
    public class ParsingException : Exception
    {
        public ParsingException() : base() { }
        public ParsingException(string message) : base(message) { }
        public ParsingException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    // Сторонний класс с методом Parse
    public class JsonParser
    {
        public void Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new Exception("JSON строка пуста");

            if (!json.StartsWith("{") || !json.EndsWith("}"))
                throw new Exception("JSON должен начинаться с { и заканчиваться }");
        }
    }

    // Класс DataProcessor
    public class DataProcessor
    {
        public void ProcessJson(string json)
        {
            try
            {
                JsonParser parser = new JsonParser();
                parser.Parse(json);
                Console.WriteLine("JSON успешно обработан");
            }
            catch (Exception ex)
            {
                // Логирование
                string log = $"[{DateTime.Now}] {ex.Message}\nСтек: {ex.StackTrace}\n\n";
                File.AppendAllText("log.txt", log);

                // Оборачивание в ParsingException
                throw new ParsingException("Ошибка при парсинге JSON", ex);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DataProcessor processor = new DataProcessor();

            // Тест 1: Некорректный JSON (нет скобок)
            try
            {
                Console.WriteLine("Тест 1 - Попытка обработать: test");
                processor.ProcessJson("test");
            }
            catch (ParsingException ex)
            {
                Console.WriteLine($"Перехвачено в Main: {ex.Message}");
                Console.WriteLine($"Причина: {ex.InnerException.Message}\n");
            }

            // Тест 2: Корректный JSON
            try
            {
                Console.WriteLine("Тест 2 - Попытка обработать: {}");
                processor.ProcessJson("{}");
                Console.WriteLine();
            }
            catch (ParsingException ex)
            {
                Console.WriteLine($"Перехвачено в Main: {ex.Message}\n");
            }

            // Тест 3: Пустая строка
            try
            {
                Console.WriteLine("Тест 3 - Попытка обработать: (пустая строка)");
                processor.ProcessJson("");
            }
            catch (ParsingException ex)
            {
                Console.WriteLine($"Перехвачено в Main: {ex.Message}");
                Console.WriteLine($"Причина: {ex.InnerException.Message}\n");
            }
        }
    }
}