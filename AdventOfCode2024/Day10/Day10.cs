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
    private static HashSet<Vector2> pathsP1 = [];
    private static List<Vector2> pathsP2 = [];

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
        int distinctPathScores = 0;

        foreach(Vector2 high in highestList)
        {
            CalculateTrail(topoMap, high, 8);
            trailheadScores += pathsP1.Count;
            pathsP1 = [];
            distinctPathScores += pathsP2.Count;
            pathsP2 = [];
        }

        Console.WriteLine($"Task 1: {trailheadScores}");
        Console.WriteLine($"Task 2: {distinctPathScores}");
    }

    private static void CalculateTrail(Dictionary<int, List<Vector2>> topoMap, Vector2 currentPos, int lowerHeight)
    {
        if (lowerHeight == -1)
        {
            pathsP1.Add(currentPos);
            pathsP2.Add(currentPos);
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
