class Program
{
    static void Main()
    {
        System.Console.WriteLine(Fibonacci(6));  // → 8
        System.Console.WriteLine(Fibonacci(1));  // → 1
        System.Console.WriteLine(Fibonacci(2));  // → 1
        System.Console.WriteLine(Fibonacci(10)); // → 55
    }

    static int Fibonacci(int n)
    {
        if (n == 1 || n == 2)
        {
            return 1;
        }
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }
}