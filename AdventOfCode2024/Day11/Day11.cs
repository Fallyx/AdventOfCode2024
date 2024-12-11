namespace AdventOfCode2024.Day11;

internal class Day11
{
    private const string inputPath = @"Day11/Input.txt";
    private const int blinkAmountP1 = 25;
    private const int blinkAmountP2 = 75;
    private static Dictionary<StoneMemo, long> memo = [];

    internal static void Task1and2()
    {
        int[] input = File.ReadLines(inputPath).First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        long count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            count += Cycle(input[i], 0, blinkAmountP1);
        }
        Console.WriteLine($"Task 1: {count}");

        memo = [];
        count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            count += Cycle(input[i], 0, blinkAmountP2);
        }
        Console.WriteLine($"Task 2: {count}");
    }

    private static long MemoCycle(long pebble, int blinks, int maxBlinks)
    {
        StoneMemo p = new(pebble, blinks);
        if (memo.TryGetValue(p, out long value))
            return value;

        long result = Cycle(pebble, blinks, maxBlinks);
        memo.Add(p, result);
        return result;
    }

    private static long Cycle(long stoneNumber, int blinks, int maxBlinks)
    {
        if (blinks == maxBlinks)
            return 1;

        if (stoneNumber == 0)
            return MemoCycle(1, blinks+1, maxBlinks);
        else if (stoneNumber.ToString().Length % 2 == 0)
        {
            string numStr = stoneNumber.ToString();
            int midIdx = numStr.Length / 2;
            long numLeft = long.Parse(numStr[..midIdx]);
            long numRight = long.Parse(numStr[midIdx..]);

            return MemoCycle(numLeft, blinks + 1, maxBlinks) + MemoCycle(numRight, blinks + 1, maxBlinks);
        }

        return MemoCycle(stoneNumber * 2024, blinks+1, maxBlinks);
    }

    internal record StoneMemo(long number, int blinks) { }
}
