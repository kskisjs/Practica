// Абстрактный класс BankAccount
abstract class BankAccount
{
    public string AccountNumber;
    public double Balance;
    public string OwnerName;

    public BankAccount(string accountNumber, double balance, string ownerName)
    {
        AccountNumber = accountNumber;
        Balance = balance;
        OwnerName = ownerName;
    }
}

// Sealed класс SavingsAccount (сберегательный)
sealed class SavingsAccount : BankAccount
{
    public SavingsAccount(string accountNumber, double balance, string ownerName)
        : base(accountNumber, balance, ownerName) { }
}

// Sealed класс CheckingAccount (расчетный)
sealed class CheckingAccount : BankAccount
{
    public CheckingAccount(string accountNumber, double balance, string ownerName)
        : base(accountNumber, balance, ownerName) { }
}

// Модельный класс Bank
class Bank
{
    public BankAccount[] Accounts;

    public Bank(BankAccount[] accounts)
    {
        Accounts = accounts;
    }

    // Метод 1: находит клиента с самым большим балансом
    public string GetRichestClient()
    {
        string richest = "";
        double maxBalance = 0;

        foreach (BankAccount acc in Accounts)
        {
            if (acc.Balance > maxBalance)
            {
                maxBalance = acc.Balance;
                richest = acc.OwnerName;
            }
        }
        return richest;
    }

    // Метод 2: вычисляет общий баланс всех клиентов
    public double GetTotalBankBalance()
    {
        double total = 0;
        foreach (BankAccount acc in Accounts)
        {
            total += acc.Balance;
        }
        return total;
    }
}

class Program
{
    static void Main()
    {
        Random rnd = new Random();
        string[] names = { "Иван Петров", "Мария Иванова", "Алексей Сидоров", "Елена Смирнова", "Дмитрий Козлов" };

        // Создаем рандомный массив счетов
        BankAccount[] accounts = new BankAccount[5];
        for (int i = 0; i < accounts.Length; i++)
        {
            string number = "ACC" + (i + 1).ToString("000");
            double balance = rnd.Next(10000, 100000);
            string name = names[rnd.Next(names.Length)];

            // Чередуем типы счетов
            if (i % 2 == 0)
                accounts[i] = new SavingsAccount(number, balance, name);
            else
                accounts[i] = new CheckingAccount(number, balance, name);
        }

        // Создаем банк
        Bank bank = new Bank(accounts);

        // Выводим все счета
        Console.WriteLine("=== Все счета ===");
        foreach (BankAccount acc in accounts)
        {
            Console.WriteLine($"{acc.OwnerName}: {acc.Balance} руб. ({acc.AccountNumber})");
        }

        // Выводим результаты
        Console.WriteLine($"\nСамый богатый клиент: {bank.GetRichestClient()}");
        Console.WriteLine($"Общий баланс всех клиентов: {bank.GetTotalBankBalance()} руб.");
    }
}