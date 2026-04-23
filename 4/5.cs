using System;

class Program
{
    static void Main()
    {
        Animal dog = new Dog();
        Animal cat = new Cat();

        dog.MakeSound();
        dog.Sleep();

        Console.WriteLine();

        cat.MakeSound();
        cat.Sleep();
    }
}

abstract class Animal
{
    public abstract void MakeSound();

    public virtual void Sleep()
    {
        Console.WriteLine("Животное спит");
    }
}

class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }

    public override void Sleep()
    {
        Console.WriteLine("Собака спит, свернувшись калачиком");
    }
}

class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Meow!");
    }

    public override void Sleep()
    {
        Console.WriteLine("Кошка спит, свернувшись клубочком");
    }
}