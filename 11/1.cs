using System;

// Интерфейс продукта
public interface IBankCard
{
    void Use();
}

// Конкретные продукты
public class CreditCard : IBankCard
{
    public void Use() => Console.WriteLine("Оплата кредитной картой");
}

public class DebitCard : IBankCard
{
    public void Use() => Console.WriteLine("Оплата дебетовой картой");
}

public class VirtualCard : IBankCard
{
    public void Use() => Console.WriteLine("Оплата виртуальной картой");
}

// Абстрактная фабрика
public abstract class BankCardFactory
{
    public abstract IBankCard CreateCard();
}

// Конкретные фабрики
public class CreditCardFactory : BankCardFactory
{
    public override IBankCard CreateCard() => new CreditCard();
}

public class DebitCardFactory : BankCardFactory
{
    public override IBankCard CreateCard() => new DebitCard();
}

public class VirtualCardFactory : BankCardFactory
{
    public override IBankCard CreateCard() => new VirtualCard();
}

// Демонстрация
class Program
{
    static void Main()
    {
        BankCardFactory factory = new CreditCardFactory();
        IBankCard card = factory.CreateCard();
        card.Use();

        factory = new DebitCardFactory();
        card = factory.CreateCard();
        card.Use();

        factory = new VirtualCardFactory();
        card = factory.CreateCard();
        card.Use();
    }
}