
class Program
{
    static void Main()
    {
        Console.Write("Введите двузначное число: ");
        int number = int.Parse(Console.ReadLine());

        int firstDigit = number / 10;      // первая цифра
        int secondDigit = number % 10;     // вторая цифра
        int product = firstDigit * secondDigit;

        Console.WriteLine($"Произведение цифр числа {number} равно {product}");
    }
}