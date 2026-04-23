class Person
{
    public string Name;
    public int Age;

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Print()
    {
        Console.WriteLine($"{Name} - {Age} лет");
    }
}

static class ArrayUtils
{
    public static void SortByName(Person[] persons)
    {
        for (int i = 0; i < persons.Length - 1; i++)
        {
            for (int j = i + 1; j < persons.Length; j++)
            {
                if (persons[i].Name.CompareTo(persons[j].Name) > 0)
                {
                    Person temp = persons[i];
                    persons[i] = persons[j];
                    persons[j] = temp;
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Random rnd = new Random();
        string[] names = { "Анна", "Борис", "Иван", "Мария", "Ольга", "Петр", "Светлана", "Дмитрий", "Елена", "Кирилл" };

        // Создаем рандомный массив из 7 человек
        Person[] people = new Person[7];
        for (int i = 0; i < people.Length; i++)
        {
            string name = names[rnd.Next(names.Length)];
            int age = rnd.Next(18, 60);
            people[i] = new Person(name, age);
        }

        // Выводим до сортировки
        Console.WriteLine("До сортировки:");
        foreach (Person p in people)
        {
            p.Print();
        }

        // Сортируем
        ArrayUtils.SortByName(people);

        // Выводим после сортировки
        Console.WriteLine("\nПосле сортировки по имени:");
        foreach (Person p in people)
        {
            p.Print();
        }
    }
}