namespace AdventOfCode2024.Day13;

internal class Day13
{
    private const string inputPath = @"Day13/Input.txt";
    private const int aTokenMultiplier = 3;
    private const long priceMultiplier = 10000000000000;

    internal static void Task1and2()
    {
        Vector2Long buttonA = new();
        Vector2Long buttonB = new();
        long totalTokensP1 = 0;
        long totalTokensP2 = 0;

        foreach (string line in File.ReadLines(inputPath))
        {
            if (line.Length == 0)
                continue;

            string[] splitted = line.Split([':', ' ', ',', '+', '='], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (splitted[1] == "A")
                buttonA = new(int.Parse(splitted[3]), int.Parse(splitted[5]));
            else if (splitted[1] == "B")
                buttonB = new(int.Parse(splitted[3]), int.Parse(splitted[5]));
            else
            {
                Vector2Long price = new(long.Parse(splitted[2]), long.Parse(splitted[4]));
                totalTokensP1 += CalcTokens(buttonA, buttonB, price);
                totalTokensP2 += CalcTokens(buttonA, buttonB, price, true);
            }
        }

        Console.WriteLine($"Task 1: {totalTokensP1}");
        Console.WriteLine($"Task 2: {totalTokensP2}");
    }

    private static long CalcTokens(Vector2Long buttonA, Vector2Long buttonB, Vector2Long price, bool isConversionError = false)
    {
        if (isConversionError)
            price = new(price.X + priceMultiplier, price.Y + priceMultiplier);

        long determinant = buttonA.X * buttonB.Y - buttonA.Y * buttonB.X;
        long aTokens = price.X * buttonB.Y - price.Y * buttonB.X;
        long bTokens = buttonA.X * price.Y - buttonA.Y * price.X;

        if (aTokens % determinant != 0 || bTokens % determinant != 0)
            return 0;

        aTokens /= determinant;
        bTokens /= determinant;

        if (!isConversionError && (aTokens > 100 || bTokens > 100))
            return 0;

        return aTokens * aTokenMultiplier + bTokens;
    }

    internal record Vector2Long(long X = 0, long Y = 0);
}
