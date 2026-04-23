int[][] arr = new int[][]
{
    new int[] { 1, 2, 3 },
    new int[] { 5, 5, 5 },
    new int[] { 10, 20, 30 }
};

int maxSum = 0;
int index = 0;

for (int i = 0; i < arr.Length; i++)
{
    int sum = arr[i][0] + arr[i][1] + arr[i][2];
    Console.WriteLine($"Строка {i + 1}: сумма = {sum}");

    if (sum > maxSum)
    {
        maxSum = sum;
        index = i;
    }
}

Console.WriteLine($"\nСамая большая сумма у строки {index + 1}: {maxSum}");