
class Program
{
    static void Main()
    {
        int a = int.Parse(Console.ReadLine());
        int b = int.Parse(Console.ReadLine());
        int c = int.Parse(Console.ReadLine());
        int s = a + b + c;
        Console.WriteLine($"{a}+{b}+{c}={s}");
        Console.WriteLine($"{c}+{b}+{a}={s}");
    }
}