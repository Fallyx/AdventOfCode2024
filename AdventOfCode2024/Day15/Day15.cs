using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day15;

internal class Day15
{
    private const string inputPath = @"Day15/Input.txt";
    private static int xMax;
    private static int yMax;
    private static readonly List<Vector2> dirs =
    [
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
    ];

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        xMax = lines.First().Length - 1;
        yMax = lines.Count(l => l.StartsWith('#')) - 1;
        HashSet<Vector2> walls = [];
        List<Vector2> boxes = [];
        Vector2 robotPos = new();
        string commands;
        bool isMap = true;

        StringBuilder sb = new();
        int y = 0;
        foreach(string line in lines)
        {
            if (line.Length == 0)
                isMap = false;
            else if (isMap)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        walls.Add(new Vector2(x, y));
                    else if (line[x] == 'O')
                        boxes.Add(new Vector2(x, y));
                    else if (line[x] == '@')
                        robotPos = new Vector2(x, y);
                }
                y++;
            }
            else
            {
                sb.Append(line);
            }
        }
        commands = sb.ToString();

        PrintMap(robotPos, boxes, walls);

        foreach(char c in commands)
        {
            Vector2 dir = new();
            if (c == '^')
                dir = dirs[0];
            else if (c == '>')
                dir = dirs[3];
            else if (c == 'v')
                dir = dirs[1];
            else if (c == '<')
                dir = dirs[2];

        //    Console.WriteLine($"command={c}, dir={dir}");
            Vector2 nextPos = robotPos + dir;
            if (CanMove(nextPos, dir, boxes, walls))
                robotPos = nextPos;

        //    PrintMap(robotPos, boxes, walls);
        }

        int gpsSum = 0;

        foreach(Vector2 box in boxes)
        {
            gpsSum += (int)(box.X + box.Y * 100);
        }

        Console.WriteLine($"Task 1: {gpsSum}");
    }

    private static bool CanMove(Vector2 nextPos, Vector2 dir, List<Vector2> boxes, HashSet<Vector2> walls)
    {
        if (walls.Contains(nextPos))
            return false;

        if (!boxes.Contains(nextPos))
            return true;
        else
        {
            int idx = boxes.IndexOf(nextPos);
            if (CanMove(nextPos + dir, dir, boxes, walls))
            {
                boxes[idx] = new(nextPos.X + dir.X, nextPos.Y + dir.Y);
                return true;
            }
            else
                return false;
        }
    }

    private static void PrintMap(Vector2 robotPos, List<Vector2> boxes, HashSet<Vector2> walls)
    {
        for (int y = 0; y <= yMax; y++)
        {
            for (int x = 0; x <= xMax; x++)
            {
                Vector2 currentPos = new(x, y);
                if (currentPos == robotPos)
                    Console.Write('@');
                else if (boxes.Contains(currentPos))
                    Console.Write('O');
                else if (walls.Contains(currentPos))
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n\n");
    }
}
