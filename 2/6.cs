while (true)
{
    Console.Write("Введите строку (или 'выход' для завершения): ");
    string text = Console.ReadLine();

    if (text == "выход" || text == "exit")
        break;

    bool hasDigit = false;
    foreach (char c in text)
    {
        if (c >= '0' && c <= '9')
        {
            hasDigit = true;
            break;
        }
    }

    if (hasDigit)
        Console.WriteLine("В строке есть цифры\n");
    else
        Console.WriteLine("В строке нет цифр\n");
}