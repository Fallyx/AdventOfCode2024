using System.Numerics;

namespace AdventOfCode2024.Day18;

internal class Day18
{
    private const string inputPath = @"Day18/Input.txt";
    private const int maxX = 70;
    private const int maxY = 70;
    private const int linesP1 = 1024;
    private static readonly List<Vector2> dirs =
    [
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
    ];

    internal static void Task1and2()
    {
        Dictionary<Vector2, int> visited = [];

        foreach (string line in File.ReadLines(inputPath))
        {
            int[] coords = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            visited.Add(new(coords[0], coords[1]), int.MaxValue);
        }

        int take = linesP1;
        int steps = FindPath(visited.Take(take).ToDictionary());
        Console.WriteLine($"Task 1: {steps}");

        while (steps != 0)
        {
            take++;
            steps = FindPath(visited.Take(take).ToDictionary());
        }

        Vector2 blockingCoord = visited.Take(take).Last().Key;
        Console.WriteLine($"Task 2: {blockingCoord.X},{blockingCoord.Y}");
    }

    private static int FindPath(Dictionary<Vector2, int> visited)
    {
        Node currentNode = new(new(), 0);
        visited.Add(currentNode.Pos, currentNode.Score);
        Queue<Node> queue = new();
        queue.Enqueue(currentNode);

        while (queue.Count > 0)
        {
            currentNode = queue.Dequeue();
            int newScore = currentNode.Score + 1;

            Vector2 north = currentNode.Pos + dirs[0];
            if (!visited.ContainsKey(north) && !IsOuterBounds(north))
            {
                queue.Enqueue(new(north, newScore));
                visited.Add(north, newScore);
            }

            Vector2 south = currentNode.Pos + dirs[1];
            if (!visited.ContainsKey(south) && !IsOuterBounds(south))
            {
                queue.Enqueue(new(south, newScore));
                visited.Add(south, newScore);
            }

            Vector2 east = currentNode.Pos + dirs[2];
            if (!visited.ContainsKey(east) && !IsOuterBounds(east))
            {
                queue.Enqueue(new(east, newScore));
                visited.Add(east, newScore);
            }

            Vector2 west = currentNode.Pos + dirs[3];
            if (!visited.ContainsKey(west) && !IsOuterBounds(west))
            {
                queue.Enqueue(new(west, newScore));
                visited.Add(west, newScore);
            }
        }

        visited.TryGetValue(new Vector2(maxX, maxY), out int steps);
        return steps;
    }

    private static bool IsOuterBounds(Vector2 position)
    {
        return (position.X < 0 || position.X > maxX || position.Y < 0 || position.Y > maxY);
    }

    internal record Node(Vector2 Pos, Int32 Score)
    {
        public Int32 Score { get; set; } = Score;
    }
}
