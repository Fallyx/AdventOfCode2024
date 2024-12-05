namespace AdventOfCode2024.Day05;

internal class Day05
{
    const string inputPath = @"Day05/Input.txt";
    private static readonly Dictionary<int, List<int>> pageOrderingRules = [];

    internal static void Task1and2()
    {
        bool readPageOrderRules = true;
        int midPageNumsP1 = 0;
        int midPageNumsP2 = 0;

        foreach (string line in File.ReadLines(inputPath))
        {
            if (line.Length == 0)
            {
                readPageOrderRules = false;
                continue;
            }

            if (readPageOrderRules)
            {
                int[] nums = line.Split('|').Select(int.Parse).ToArray();

                if (!pageOrderingRules.TryAdd(nums[0], [nums[1]]))
                    pageOrderingRules[nums[0]].Add(nums[1]);
            }
            else
            {
                int[] nums = line.Split(',').Select(int.Parse).ToArray();
                int[] numsP2 = line.Split(',').Select(int.Parse).ToArray();

                bool isValidPage = true;

                for (int i = nums.Length - 1; i >= 1; i--)
                {
                    for (int x = i - 1; x >= 0; x--)
                    {
                        if (pageOrderingRules.TryGetValue(nums[i], out List<int> vals) && vals.Contains(nums[x]))
                        {
                            isValidPage = false;
                            (nums[i], nums[x]) = (nums[x], nums[i]);
                            i++;
                            break;
                        }
                    }
                }

                if (isValidPage)
                    midPageNumsP1 += nums[nums.Length / 2];
                else
                    midPageNumsP2 += nums[nums.Length / 2];
            }
        }

        Console.WriteLine($"Task 1: {midPageNumsP1}");
        Console.WriteLine($"Task 2: {midPageNumsP2}");
    }
}
