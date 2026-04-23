class Task7
{
    static void Main()
    {
        Console.WriteLine("Числа от 80 до 10 с шагом 2 (в столбик):");

        // Способ 1: for
        for (int i = 80; i >= 10; i -= 2)
        {
            Console.WriteLine(i);
        }

        // Способ 2: while (закомментирован)
        // int j = 80;
        // while (j >= 10)
        // {
        //     Console.WriteLine(j);
        //     j -= 2;
        // }

        // Способ 3: do while (закомментирован)
        // int k = 80;
        // do
        // {
        //     Console.WriteLine(k);
        //     k -= 2;
        // } while (k >= 10);
    }
}