using System.Numerics;

namespace AdventOfCode2024.Day06;

internal class Day06
{
    const string inputPath = @"Day06/Input.txt";
    static int xMax;
    static int yMax;
    private static readonly Vector2[] cycle = [new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0)];

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        HashSet<Vector2> obstacles = [];

        xMax = lines.First().Length - 1;
        yMax = lines.Count() - 1;

        Vector2 currentPos = new(-1, -1);

        int y = 0;
        foreach(string line in lines)
        {
            for (int x = 0; x <= xMax; x++)
            {
                if (line[x] == '#')
                    obstacles.Add(new Vector2(x, y));
                else if (line[x] == '^')
                    currentPos = new Vector2(x, y);
            }
            y++;
        }

        HashSet<Vector2> visited = Task1(currentPos, obstacles);
     //   Task2(currentPos, obstacles, [.. visited]);
    }

    private static HashSet<Vector2> Task1(Vector2 currentPos, HashSet<Vector2> obstacles)
    {
        HashSet<Vector2> visited = [];
        int cyclePos = 0;

        while (currentPos.X >= 0 && currentPos.X <= xMax && currentPos.Y >= 0 && currentPos.Y <= yMax)
        {
            visited.Add(currentPos);
            Vector2 nextPos = currentPos + cycle[cyclePos];
            if (obstacles.Contains(nextPos))
                cyclePos = (cyclePos + 1) % 4;
            else
                currentPos = nextPos;
        }

        Console.WriteLine($"Task 1: {visited.Count}");

        return visited;
    }

    private static void Task2(Vector2 currentPos, HashSet<Vector2> obstacles, List<Vector2> walkingPath)
    {
        HashSet<VisitedField> visited = [];
        int cyclePos = 0;

        for (int i = 1; i < walkingPath.Count; i++)
        {
            Console.WriteLine(walkingPath[i]);
        }

    }

    internal class VisitedField(Vector2 pos, Vector2 dir)
    {
        Vector2 Pos = pos;
        Vector2 Dir = dir;
    }
}
