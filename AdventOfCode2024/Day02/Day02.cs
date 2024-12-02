namespace AdventOfCode2024.Day02;

internal class Day02
{
    const string inputPath = @"Day02/Input.txt";

    internal static void Task1and2()
    {
        List<string> lines = [.. File.ReadAllLines(inputPath)];
        int safeReportsP1 = 0;
        int safeReportsP2 = 0;

        foreach(string line in lines)
        {
            List<int> nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<int> diffs = new(nums.Count - 1);

            for(int i = 0; i < nums.Count - 1; i++)
                diffs.Add(nums[i] - nums[i + 1]);

            if (diffs.All(n => n >= 1 && n <= 3) || diffs.All(n => n <= -1 && n >= -3))
            {
                safeReportsP1++;
                safeReportsP2++;
                continue;
            }

            for (int i = 0; i < nums.Count; i++)
            {
                List<int> subNums = new(nums);
                subNums.RemoveAt(i);
                List<int> subDiffs = new(subNums.Count - 1);

                for (int x = 0; x < subNums.Count - 1; x++)
                    subDiffs.Add(subNums[x] - subNums[x + 1]);

                if (subDiffs.All(n => n >= 1 && n <= 3) || subDiffs.All(n => n <= -1 && n >= -3))
                {
                    safeReportsP2++;
                    break;
                } 
            }
        }

        Console.WriteLine($"Task 1: {safeReportsP1}");
        Console.WriteLine($"Task 2: {safeReportsP2}");
    }
}
