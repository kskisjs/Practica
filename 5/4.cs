using System;

class Program
{
    static void Main()
    {
        NotificationService service = new NotificationService();

        IEmailNotifier emailNotifier = service;
        ISmsNotifier smsNotifier = service;

        Console.WriteLine("=== СИСТЕМА УВЕДОМЛЕНИЙ ===\n");

        // Только Email
        Console.WriteLine("--- ОТПРАВКА EMAIL ---");
        emailNotifier.SendNotification("Ваш заказ подтвержден");

        // Только SMS
        Console.WriteLine("\n--- ОТПРАВКА SMS ---");
        smsNotifier.SendNotification("Ваш заказ подтвержден");
    }
}

interface IEmailNotifier
{
    void SendNotification(string message);
}

interface ISmsNotifier
{
    void SendNotification(string message);
}

class NotificationService : IEmailNotifier, ISmsNotifier
{
    void IEmailNotifier.SendNotification(string message)
    {
        Console.WriteLine($"[EMAIL] Отправлено письмо: {message}");
    }

    void ISmsNotifier.SendNotification(string message)
    {
        Console.WriteLine($"[SMS] Отправлено сообщение: {message}");
    }
}