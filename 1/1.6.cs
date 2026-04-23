class Task6
{
    static void Main()
    {
        Console.Write("Введите год: ");
        int year = Convert.ToInt32(Console.ReadLine());

        string[] animals = { "Обезьяна", "Петух", "Собака", "Свинья",
                             "Крыса", "Бык", "Тигр", "Заяц",
                             "Дракон", "Змея", "Лошадь", "Коза" };

        int index = year % 12;
        string animal = animals[index];

        Console.WriteLine($"Год {year} - символ: {animal}");
    }
}