using System;

class Program
{
    static void Main()
    {
        // Создаём массив животных
        Animal[] animals = new Animal[3];

        animals[0] = new Dog();
        animals[1] = new Cat();
        animals[2] = new Cow();

        // Выводим информацию о всех животных
        Console.WriteLine("=== ЗВУКИ ЖИВОТНЫХ ===\n");

        for (int i = 0; i < animals.Length; i++)
        {
            animals[i].DisplayInfo();
            Console.WriteLine();
        }
    }
}

// Абстрактный базовый класс
abstract class Animal
{
    public abstract void MakeSound();
    public abstract string GetName();

    // Метод для вывода полной информации
    public void DisplayInfo()
    {
        Console.Write($"{GetName()}: ");
        MakeSound();
    }
}

// Наследник 1 - Собака
class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof! Woof!");
    }

    public override string GetName()
    {
        return "Собака";
    }
}

// Наследник 2 - Кошка
class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Meow! Meow!");
    }

    public override string GetName()
    {
        return "Кошка";
    }
}

// Наследник 3 - Корова
class Cow : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Moo! Moo!");
    }

    public override string GetName()
    {
        return "Корова";
    }
}