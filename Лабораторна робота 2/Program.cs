using System;
using System.Linq;
using System.Threading;

public class MinimumElementFinder
{
    private static int[] array; // Великий масив

    private static int[] minimumValues;
    private static int numThreads = 1000; // Кількість потоків

    public static void Main(string[] args)
    {
        // Ініціалізуємо великий масив з 10 000 000 елементів
        array = GenerateArray(10_000_000);

        // Замінюємо довільний елемент на випадкове від'ємне число
        ReplaceRandomElementWithNegative();

        minimumValues = new int[numThreads];

        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int startIndex = i * (array.Length / numThreads);
            int endIndex = (i == numThreads - 1) ? array.Length : (i + 1) * (array.Length / numThreads);
            int threadIndex = i;

            threads[i] = new Thread(() =>
            {
                int min = array[startIndex];
                int minIndex = startIndex;

                for (int j = startIndex + 1; j < endIndex; j++)
                {
                    if (array[j] < min)
                    {
                        min = array[j];
                        minIndex = j;
                    }
                }

                minimumValues[threadIndex] = min;

                // Синхронізовано виводимо значення мінімального елементу та його індекс
                lock (typeof(MinimumElementFinder))
                {
                    Console.WriteLine($"Thread {threadIndex}: Minimum element: {min}, Index: {minIndex}");
                }
            });

            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        int overallMin = minimumValues.Min();
        int overallMinIndex = Array.IndexOf(minimumValues, overallMin);
        Console.WriteLine($"Overall Minimum element: {overallMin}, Index: {overallMinIndex}");
    }

    private static int[] GenerateArray(int length)
    {
        int[] arr = new int[length];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = i;
        }
        return arr;
    }

    private static void ReplaceRandomElementWithNegative()
    {
        Random random = new Random();
        int randomIndex = random.Next(array.Length);
        int randomNegativeNumber = -random.Next(1, 1000); // Випадкове від'ємне число
        array[randomIndex] = randomNegativeNumber;
    }
}

