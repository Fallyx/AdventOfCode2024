using System.Numerics;

namespace AdventOfCode2024.Day10;

internal class Day10
{
    private const string inputPath = @"Day10/Input.txt";
    private static readonly List<Vector2> neighbors =
    [
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
    ];

    private static List<Vector2> validPaths = [];

    internal static void Task1and2()
    {
        Dictionary<int, List<Vector2>> topoMap = [];
        int y = 0;

        foreach (string line in File.ReadLines(inputPath))
        {
            for (int x = 0; x < line.Length; x++)
            {
                int key = int.Parse(line[x].ToString());
                Vector2 val = new(x, y);

                if (!topoMap.TryAdd(key, [val])) {
                    topoMap[key].Add(val);
                }
            }
            y++;
        }

        List<Vector2> highestList = topoMap[9];
        int trailheadScores = 0;
        int trailheadRatings = 0;

        foreach(Vector2 currentPos in highestList)
        {
            CalculateTrail(topoMap, currentPos, 8);
            trailheadScores += validPaths.Distinct().Count();
            trailheadRatings += validPaths.Count;
            validPaths = [];
        }

        Console.WriteLine($"Task 1: {trailheadScores}");
        Console.WriteLine($"Task 2: {trailheadRatings}");
    }

    private static void CalculateTrail(Dictionary<int, List<Vector2>> topoMap, Vector2 currentPos, int lowerHeight)
    {
        if (lowerHeight == -1)
        {
            validPaths.Add(currentPos);
            return;
        }

        List<Vector2> lowerHeightList = topoMap[lowerHeight];

        foreach(Vector2 neighbor in neighbors)
        {
            Vector2 possiblePos = currentPos + neighbor;
            if (lowerHeightList.Contains(possiblePos))
                CalculateTrail(topoMap, possiblePos, lowerHeight - 1);
        }
    }
}
