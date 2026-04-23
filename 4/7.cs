using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите животное:");
        Console.WriteLine("1 - Dog");
        Console.WriteLine("2 - Cat");
        Console.WriteLine("3 - Cow");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();
        string animal = "";

        if (choice == "1")
        {
            animal = "Dog";
        }
        else if (choice == "2")
        {
            animal = "Cat";
        }
        else if (choice == "3")
        {
            animal = "Cow";
        }
        else
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        Console.Write("Хотите задать свой звук? (да/нет): ");
        string custom = Console.ReadLine();

        if (custom == "да")
        {
            Console.Write("Введите звук: ");
            string noise = Console.ReadLine();
            Console.WriteLine(GetAnimalSound(animal, noise));
        }
        else
        {
            Console.WriteLine(GetAnimalSound(animal));
        }
    }

    static string GetAnimalSound(string animal)
    {
        if (animal == "Dog")
        {
            return "Woof";
        }
        else if (animal == "Cat")
        {
            return "Meow";
        }
        else if (animal == "Cow")
        {
            return "Moo";
        }
        else
        {
            return "Unknown animal";
        }
    }

    static string GetAnimalSound(string animal, string noise)
    {
        return noise;
    }
}