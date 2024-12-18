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

            for (int i = 0; i < dirs.Count; i++)
            {
                Vector2 neighbor = currentNode.Pos + dirs[i];
                if (!visited.ContainsKey(neighbor) && !(neighbor.X < 0 || neighbor.X > maxX || neighbor.Y < 0 || neighbor.Y > maxY))
                {
                    queue.Enqueue(new(neighbor, newScore));
                    visited.Add(neighbor, newScore);
                }
            }
        }

        visited.TryGetValue(new Vector2(maxX, maxY), out int steps);
        return steps;
    }

    internal record Node(Vector2 Pos, Int32 Score);
}
