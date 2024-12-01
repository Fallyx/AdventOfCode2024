namespace AdventOfCode2024.Day01;

internal class Day01
{
    const string inputPath = @"Day01/Input.txt";

    internal static void Task1and2()
    {
        List<string> lines = [.. File.ReadAllLines(inputPath)];
        List<int> left = new(lines.Count);
        List<int> right = new(lines.Count);

        foreach(string line in lines)
        {
            string[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(numbers[0]));
            right.Add(int.Parse(numbers[1]));
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
