using System;

namespace BankWithdrawTask
{
    // Пользовательское исключение
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base() { }
        public InsufficientFundsException(string message) : base(message) { }
        public InsufficientFundsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    // Класс BankAccount
    public class BankAccount
    {
        public decimal Balance { get; private set; }

        public BankAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                throw new InsufficientFundsException(
                    $"Недостаточно средств. Баланс: {Balance} руб., запрошено: {amount} руб.");
            }

            Balance -= amount;
            Console.WriteLine($"Снято {amount} руб. Остаток: {Balance} руб.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите начальный баланс: ");
            decimal initialBalance = decimal.Parse(Console.ReadLine());

            BankAccount account = new BankAccount(initialBalance);

            while (true)
            {
                Console.WriteLine($"\nТекущий баланс: {account.Balance} руб.");
                Console.Write("Введите сумму для снятия (0 - выход): ");
                decimal amount = decimal.Parse(Console.ReadLine());

                if (amount == 0)
                    break;

                try
                {
                    account.Withdraw(amount);
                }
                catch (InsufficientFundsException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Программа завершена");
        }
    }
}