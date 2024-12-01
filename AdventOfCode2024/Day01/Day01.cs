using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day01;

internal partial class Day01
{
    const string inputPath = @"Day01/Input.txt";
    [GeneratedRegex(@"(\d+)\s+(\d+)")]
    private static partial Regex numberRegex();

    internal static void Task1()
    {
        List<string> lines = [.. File.ReadAllLines(inputPath)];
        List<int> left = new(lines.Count);
        List<int> right = new(lines.Count);
        
        foreach(string line in lines)
        {
            Match match = numberRegex().Match(line);
            if (match.Success)
            {
                left.Add(int.Parse(match.Groups[1].ToString()));
                right.Add(int.Parse(match.Groups[2].ToString()));
            }
        }

        left.Sort();
        right.Sort();

        int totDistance = 0;
        int simScore = 0;

        for(int i = 0; i < left.Count; i++)
        {
            totDistance += Math.Abs(left[i] - right[i]);
            simScore += left[i] * right.Where(num => num == left[i]).Count();
        }

        Console.WriteLine($"Task 1: {totDistance}");
        Console.WriteLine($"Task 2: {simScore}");
    }
}
