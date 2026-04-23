class A
{
    public int a;
    public int b;

    public A(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public int Raznost()
    {
        return a - b;
    }

    public double Vyrazhenie()
    {
        return (double)(a + b) / (a - b);
    }
}

class Program
{
    static void Main()
    {
        Random rnd = new Random();

        // Рандомные числа от 1 до 20
        int a = rnd.Next(1, 21);
        int b = rnd.Next(1, 21);

        // Чтобы не было деления на ноль
        while (a == b)
        {
            b = rnd.Next(1, 21);
        }

        A obj = new A(a, b);

        Console.WriteLine($"a = {obj.a}, b = {obj.b}");
        Console.WriteLine($"Разность: {obj.Raznost()}");
        Console.WriteLine($"(a+b)/(a-b) = {obj.Vyrazhenie():F2}");
    }
}