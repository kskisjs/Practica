
class Program
{
    static void Main()
    {
        Console.Write("Введите четырехзначное число: ");
        int n = int.Parse(Console.ReadLine());

        int a = n / 1000;
        int b = (n / 100) % 10;
        int c = (n / 10) % 10;
        int d = n % 10;

        int product = a * b * c * d;

        Console.WriteLine($"Произведение цифр: {product}");
    }
}