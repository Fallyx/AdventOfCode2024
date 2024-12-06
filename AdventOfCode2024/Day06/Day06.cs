using System.Numerics;

namespace AdventOfCode2024.Day06;

internal class Day06
{
    const string inputPath = @"Day06/Input.txt";
    static int xMax;
    static int yMax;
    private static readonly Vector2[] cycles = [new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0)];

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

        List<VisitedFieldRec> visitedFieldsList = Task1(currentPos, obstacles);
        Task2(currentPos, obstacles, visitedFieldsList);
    }

    private static List<VisitedFieldRec> Task1(Vector2 currentPos, HashSet<Vector2> obstacles)
    {
        Vector2 startPos = currentPos;
        HashSet<VisitedFieldRec> visitedFields = [];
        int cyclePos = 0;

        while (currentPos.X >= 0 && currentPos.X <= xMax && currentPos.Y >= 0 && currentPos.Y <= yMax)
        {
            visitedFields.Add(new VisitedFieldRec(currentPos, cycles[cyclePos]));
            Vector2 nextPos = currentPos + cycles[cyclePos];
            if (obstacles.Contains(nextPos))
                cyclePos = (cyclePos + 1) % 4;
            else
                currentPos = nextPos;
        }

        Console.WriteLine($"Task 1: {visitedFields.ToList().Select(vf => vf.Pos).Distinct().Count()}");

        return [.. visitedFields];
    }

    private static void Task2(Vector2 currentPos, HashSet<Vector2> obstacles, List<VisitedFieldRec> visiteds)
    {
        Vector2 startPos = currentPos;
        HashSet<Vector2> newObstacleSet = [];

        for (int i = 0; i < visiteds.Count - 1; i++)
        {
            HashSet<VisitedFieldRec> visitedFieldsWithCycle = [];
            Vector2 newObstacle = visiteds[i].Pos + visiteds[i].Dir;

            if (newObstacleSet.Contains(newObstacle))
                continue;

            currentPos = startPos;
            int cyclePos = 0;
            bool isCycle = false;

            while (currentPos.X >= 0 && currentPos.X <= xMax && currentPos.Y >= 0 && currentPos.Y <= yMax)
            {
                if (visitedFieldsWithCycle.Add(new VisitedFieldRec(currentPos, cycles[cyclePos])))
                {
                    Vector2 nextPos = currentPos + cycles[cyclePos];
                    if (obstacles.Contains(nextPos) || newObstacle == nextPos)
                        cyclePos = (cyclePos + 1) % 4;
                    else
                        currentPos = nextPos;
                }
                else
                {
                    isCycle = true;
                    break;
                }
            }

            if (isCycle)
                newObstacleSet.Add(newObstacle);
        }

        Console.WriteLine($"Task 2: {newObstacleSet.Count}");
    }

    internal record VisitedFieldRec(Vector2 Pos, Vector2 Dir) { }
}
