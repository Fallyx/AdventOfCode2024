using System.Numerics;

namespace AdventOfCode2024.Day04;

internal class Day04
{
    const string inputPath = @"Day04/Input.txt";
    private static readonly List<Vector2> neighbors =
    [
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(1, 1)
    ];

    internal static void Task1and2()
    {
        List<string> lines = [.. File.ReadAllLines(inputPath)];
        Dictionary<Vector2, char> map = [];

        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map.Add(new Vector2(x, y), lines[y][x]);
            }
        }

        int countXmas = 0;
        int countMas = 0;
        Vector2 currentPos = new(0, 0);

        for (int y = 0; y < lines.Count; y++)
        {
            currentPos.Y = y;
            for (int x = 0; x < lines[y].Length; x++)
            {
                currentPos.X = x;
                map.TryGetValue(currentPos, out char currentChar);

                if (currentChar == 'X')
                {
                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        countXmas += CheckXmas(map, currentPos, neighbors[i], 'M');
                    }
                }
                else if (currentChar == 'A')
                {
                    countMas += CheckMas(map, currentPos);
                }
            }
        }

        Console.WriteLine($"Task 1: {countXmas}");
        Console.WriteLine($"Task 2: {countMas}");
    }

    private static int CheckXmas(Dictionary<Vector2, char> map, Vector2 currentPos, Vector2 neighbor, char toFind)
    {
        Vector2 nextPos = new(currentPos.X + neighbor.X, currentPos.Y + neighbor.Y);
        if(map.TryGetValue(nextPos, out char nextChar))
        {
            if (nextChar == toFind && nextChar == 'S')
                return 1;
            else if (nextChar == toFind && nextChar == 'A')
                return CheckXmas(map, nextPos, neighbor, 'S');
            else if (nextChar == toFind && nextChar == 'M')
                return CheckXmas(map, nextPos, neighbor, 'A');
            else
                return 0;
        }

        return 0;
    }

    private static int CheckMas(Dictionary<Vector2,char> map, Vector2 currentPos)
    {
        if (map.TryGetValue(currentPos + neighbors[0], out char ul) 
            && map.TryGetValue(currentPos + neighbors[2], out char ur)
            && map.TryGetValue(currentPos + neighbors[5], out char ll)
            && map.TryGetValue(currentPos + neighbors[7], out char lr))
        {
            if (((ul == 'M' && lr == 'S') || (lr == 'M' && ul == 'S')) && ((ur == 'M' && ll == 'S') || (ll == 'M' && ur == 'S')))
                return 1;
        }

        return 0;
    }
}
