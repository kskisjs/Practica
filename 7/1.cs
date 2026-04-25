using System;

namespace BankAccountValidation
{
    // Пользовательское исключение
    public class InvalidAccountNumberException : Exception
    {
        public InvalidAccountNumberException() : base() { }
        public InvalidAccountNumberException(string message) : base(message) { }
        public InvalidAccountNumberException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    // Класс BankAccount
    public class BankAccount
    {
        public void ValidateAccount(string accountNumber)
        {
            if (accountNumber.Length != 10)
                throw new InvalidAccountNumberException("Номер счета должен состоять из 10 цифр");

            foreach (char c in accountNumber)
                if (!char.IsDigit(c))
                    throw new InvalidAccountNumberException("Номер счета должен содержать только цифры");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BankAccount account = new BankAccount();

            Console.Write("Введите номер счета (10 цифр): ");
            string input = Console.ReadLine();

            try
            {
                account.ValidateAccount(input);
                Console.WriteLine("Номер счета корректный!");
            }
            catch (InvalidAccountNumberException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}