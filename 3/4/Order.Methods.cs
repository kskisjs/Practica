using System;

public partial class Order
{
    public void Print()
    {
        Console.WriteLine($"Заказ #{OrderID}: {CustomerName} - {TotalAmount} руб.");
    }
}