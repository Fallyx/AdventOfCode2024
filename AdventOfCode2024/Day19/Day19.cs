namespace AdventOfCode2024.Day19;

internal class Day19
{
    private const string inputPath = @"Day19/Input.txt";

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);

        List<string> towels = [.. lines.First().Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)];

        long possibleDesignsP1 = 0;
        long possibleDesignsP2 = 0;
        lines = lines.Skip(2);

        foreach (string design in lines)
        {
            Dictionary<string, long> memo = [];
            long result = PossibleArrangements(design, towels, memo);
            if (result > 0)
            {
                possibleDesignsP1++;
                possibleDesignsP2 += result;
            }
        }

        Console.WriteLine($"Task 1: {possibleDesignsP1}");
        Console.WriteLine($"Task 2: {possibleDesignsP2}");
    }


    private static long PossibleArrangements(string design, List<string> availableTowels, Dictionary<string, long> memo)
    {
        if (design.Length == 0)
            return 1;

        long total = 0;
        foreach(string towel in availableTowels)
        {
            string key = $"{towel}-{design}";
            if (memo.TryGetValue(key, out long value))
                total += value;
            else if (design.StartsWith(towel))
            {
                long tot = PossibleArrangements(design[towel.Length..], availableTowels, memo);
                total += tot;
                memo.Add(key, tot);
            }
        }

        return total;
    }
}
