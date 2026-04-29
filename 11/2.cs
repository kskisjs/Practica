using System;

// Интерфейс компонента
public interface ICoffee
{
    string GetDescription();
    double GetCost();
}

// Конкретный компонент (базовый кофе)
public class BasicCoffee : ICoffee
{
    public string GetDescription() => "Черный кофе";
    public double GetCost() => 100.0;
}

// Декоратор (базовый)
public abstract class CoffeeDecorator : ICoffee
{
    protected ICoffee _coffee;
    public CoffeeDecorator(ICoffee coffee) { _coffee = coffee; }
    public abstract string GetDescription();
    public abstract double GetCost();
}

// Конкретные декораторы
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee coffee) : base(coffee) { }
    public override string GetDescription() => _coffee.GetDescription() + ", с молоком";
    public override double GetCost() => _coffee.GetCost() + 20.0;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(ICoffee coffee) : base(coffee) { }
    public override string GetDescription() => _coffee.GetDescription() + ", с сахаром";
    public override double GetCost() => _coffee.GetCost() + 10.0;
}

public class SyrupDecorator : CoffeeDecorator
{
    public SyrupDecorator(ICoffee coffee) : base(coffee) { }
    public override string GetDescription() => _coffee.GetDescription() + ", с сиропом";
    public override double GetCost() => _coffee.GetCost() + 30.0;
}

// Демонстрация
class Program
{
    static void Main()
    {
        ICoffee coffee = new BasicCoffee();
        Console.WriteLine($"{coffee.GetDescription()} - {coffee.GetCost()} руб.");

        coffee = new MilkDecorator(coffee);
        Console.WriteLine($"{coffee.GetDescription()} - {coffee.GetCost()} руб.");

        coffee = new SugarDecorator(coffee);
        Console.WriteLine($"{coffee.GetDescription()} - {coffee.GetCost()} руб.");

        coffee = new SyrupDecorator(coffee);
        Console.WriteLine($"{coffee.GetDescription()} - {coffee.GetCost()} руб.");
    }
}