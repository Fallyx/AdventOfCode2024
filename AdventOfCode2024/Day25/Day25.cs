namespace AdventOfCode2024.Day25;

internal class Day25
{
    private const string inputPath = @"Day25/Input.txt";

    internal static void Task1()
    {
        List<int[]> keysList = [];
        List<int[]> locksList = [];
        string[] keyslocks = File.ReadAllText(inputPath).Split(["\n\n", "\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (string keylock in keyslocks)
        {
            string[] split = keylock.Split(["\n", "\r\n"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            bool isLock = split[0] == "#####";
            int[] heights = new int[5];

            for (int i = 0; i < 5; i++)
            {
                List<char> charList = [split[1][i], split[2][i], split[3][i], split[4][i], split[5][i]];
                heights[i] = charList.Count(c => c == '#');
            }

            if (isLock)
                locksList.Add(heights);
            else
                keysList.Add(heights);
        }

        int validKeysLocks = 0;

        foreach (int[] lockHeights in locksList)
        {
            foreach (int[] keysHeights in keysList)
            {
                bool isValid = true;

                for(int i = 0; i < lockHeights.Length; i++)
                {
                    if (lockHeights[i] + keysHeights[i] > 5)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                    validKeysLocks++;
            }
        }

        Console.WriteLine($"Task 1: {validKeysLocks}");
    }
}
