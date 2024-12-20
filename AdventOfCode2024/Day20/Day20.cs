using System.Numerics;

namespace AdventOfCode2024.Day20;

internal class Day20
{
    private const string inputPath = @"Day20/Input.txt";
    private static readonly List<Vector2> directions = [ 
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1)
    ];

    internal static void Task1and2()
    {
        Dictionary<Vector2, int> paths = [];
        Vector2 startPos = new();

        int y = 0;
        foreach (string line in File.ReadLines(inputPath))
        {
            for(int x = 0; x < line.Length; x++)
            {
                Vector2 current = new(x, y);
                if (line[x] == '.' || line[x] == 'E')
                    paths.Add(current, int.MaxValue);
                else if (line[x] == 'S')
                {
                    paths.Add(current, 0);
                    startPos = current;
                }
            }
            y++;
        }

        TraversePath(startPos, paths);

        int shortcutsP1 = FindShortcuts(paths, 2);
        Console.WriteLine($"Task 1: {shortcutsP1}");
        int shortcutsP2 = FindShortcuts(paths, 20);
        Console.WriteLine($"Task 2: {shortcutsP2}");
    }

    private static void TraversePath(Vector2 startPos, Dictionary<Vector2, int> paths)
    {
        Queue<Vector2> queue = new();
        queue.Enqueue(startPos);

        while (queue.Count > 0)
        {
            Vector2 currentPos = queue.Dequeue();
            int newScore = paths[currentPos] + 1;

            for (int i = 0; i < directions.Count; i++)
            {
                Vector2 nextPos = currentPos + directions[i];
                if (paths.TryGetValue(nextPos, out int currentNextPosScore) && currentNextPosScore > newScore)
                {
                    paths[nextPos] = newScore;
                    queue.Enqueue(nextPos);
                }
            }
        }
    }

    private static int FindShortcuts(Dictionary<Vector2, int> paths, int shortcutLength)
    {
        int shortcuts = 0;
        HashSet<Vector2> visited = [];

        foreach (KeyValuePair<Vector2, int> nodeS in paths)
        {
            visited.Add(nodeS.Key);
            foreach(KeyValuePair<Vector2, int> nodeE in paths)
            {
                if (visited.Contains(nodeE.Key))
                    continue;

                int distance = (int)(Math.Abs(nodeS.Key.X - nodeE.Key.X) + Math.Abs(nodeS.Key.Y - nodeE.Key.Y));

                if (distance > shortcutLength)
                    continue;

                if (Math.Abs(nodeS.Value - nodeE.Value) >= 100 + distance)
                    shortcuts++;
            }
        }

        return shortcuts;
    }
}
