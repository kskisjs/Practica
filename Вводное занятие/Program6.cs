
class Program
{
    static void Main()
    {
        double x = 5;

        double term1 = Math.Exp(2 * x);                           // e^(2x)
        double term2 = Math.Exp(Math.Sqrt(Math.Abs(1 - x)));      // e^(sqrt(|1-x|))
        double term3 = (2 * Math.Pow(Math.Sin(x), 2)) / (Math.PI * x * x);  // (2 sin^2 x)/(π x^2)

        double y = term1 - term2 + term3;

        Console.WriteLine($"x = {x}");
        Console.WriteLine($"y = {y}");
        Console.WriteLine($"y = {y:F6}");
    }
}