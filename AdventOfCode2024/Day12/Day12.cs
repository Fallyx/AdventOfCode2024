using System.Numerics;

namespace AdventOfCode2024.Day12;

internal class Day12
{
    private const string inputPath = @"Day12/Input.txt";
    private static readonly List<Vector2> neighbors =
    [
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
    ];

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        Dictionary<char, List<Vector2>> gardenPos = [];
        int xMax = lines.First().Length - 1;

        int y = 0;
        foreach (string line in lines)
        {
            for (int x = 0; x <= xMax; x++)
            {
                Vector2 pos = new(x, y);
                if (!gardenPos.TryAdd(line[x], [pos]))
                    gardenPos[line[x]].Add(pos);
            }
            y++;
        }

        Console.WriteLine();
        int totalPriceP1 = 0;
        int totalPriceP2 = 0;
        foreach (KeyValuePair<char, List<Vector2>> gPos in gardenPos)
        {
            HashSet<Vector2> visited = [];
            foreach (Vector2 pos in gPos.Value)
            {
                HashSet<GardenTile> gardenTiles = [];
                int groupSize = CalcPerimeter(pos, visited, gPos.Value, gardenTiles);
                int sides = CalcSides(gardenTiles);
                int perimeterCount = gardenTiles.Select(gt => gt.FencePos).Count();

                totalPriceP1 += groupSize * perimeterCount;
                totalPriceP2 += groupSize * sides;
            }
        }

        Console.WriteLine($"Task 1: {totalPriceP1}");
        Console.WriteLine($"Task 2: {totalPriceP2}");
    }

    private static int CalcPerimeter(Vector2 currentPos, HashSet<Vector2> visited, List<Vector2> samePlants, HashSet<GardenTile> gardenTiles)
    {
        int groupSize = 0;
        if (visited.Contains(currentPos))
            return groupSize;
        visited.Add(currentPos);

        foreach (Vector2 dir in neighbors)
        {
            Vector2 neighborPos = currentPos + dir;
            if (!samePlants.Contains(neighborPos))
                gardenTiles.Add(new(currentPos, neighborPos));
            else
                groupSize += CalcPerimeter(neighborPos, visited, samePlants, gardenTiles);
        }
        return ++groupSize;
    }

    private static int CalcSides(HashSet<GardenTile> gardenTiles)
    {
        int sides = 0;
        HashSet<GardenTile> visited = [];

        foreach (GardenTile gardenTile in gardenTiles)
        {
            if (visited.Contains(gardenTile))
                continue;

            visited.Add(gardenTile);

            bool isVertical = true;
            if ((gardenTile.GardenPos - gardenTile.FencePos).X == 0)
                isVertical = false;

            List<Vector2> dirs = (isVertical ? neighbors.Take(2).ToList() : neighbors.Skip(2).ToList());

            foreach(Vector2 dir in dirs)
            {
                bool sideEnded = false;
                Vector2 possibleGardenPos = gardenTile.GardenPos;
                Vector2 possibleFencePos = gardenTile.FencePos;
                while (!sideEnded)
                {
                    possibleGardenPos += dir;
                    possibleFencePos += dir;
                    GardenTile possibleTile = new(possibleGardenPos, possibleFencePos);
                    if (gardenTiles.Contains(possibleTile))
                        visited.Add(possibleTile);
                    else
                        sideEnded = true;
                }
            }
            sides++;
        }

        return sides;
    }

    internal record GardenTile(Vector2 GardenPos, Vector2 FencePos) { }
}
