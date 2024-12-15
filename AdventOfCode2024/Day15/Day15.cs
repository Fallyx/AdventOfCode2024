using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day15;

internal class Day15
{
    private const string inputPath = @"Day15/Input.txt";
    private static readonly List<Vector2> dirs =
    [
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
    ];

    internal static void Task1and2()
    {
        Task1();
        Task2();
    }

    private static void Task1()
    {
        HashSet<Vector2> walls = [];
        List<Vector2> boxes = [];
        (Vector2 robotPos, int maxX, int maxY, string commands) = BuildMap(boxes, walls, false);

        PrintMap(robotPos, boxes, walls, maxX, maxY, false);

        RunCommands(commands, robotPos, boxes, walls, false);

        int gpsSum = 0;

        foreach (Vector2 box in boxes)
        {
            gpsSum += (int)(box.X + box.Y * 100);
        }

        Console.WriteLine($"Task 1: {gpsSum}");
    }

    private static void Task2()
    {
        HashSet<Vector2> walls = [];
        List<Vector2> boxes = [];
        (Vector2 robotPos, int maxX, int maxY, string commands) = BuildMap(boxes, walls, true);

        PrintMap(robotPos, boxes, walls, maxX, maxY, true);

        RunCommands(commands, robotPos, boxes, walls, true, maxX, maxY, true);

        PrintMap(robotPos, boxes, walls, maxX, maxY, true);

        int gpsSum = 0;

        foreach (Vector2 box in boxes)
        {
            gpsSum += (int)(box.X + box.Y * 100);
        }

        Console.WriteLine($"Task 1: {gpsSum}");
    }

    private static void RunCommands(string commands, Vector2 robotPos, List<Vector2> boxes, HashSet<Vector2> walls, bool isPart2, int maxX = 0, int maxY = 0, bool printMap = false)
    {
        foreach (char c in commands)
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

            if (printMap)
                Console.WriteLine($"command={c}, dir={dir}");

            Vector2 nextPos = robotPos + dir;
            if (CanMove(nextPos, dir, boxes, walls))
                robotPos = nextPos;

            if (printMap)
                PrintMap(robotPos, boxes, walls, maxX, maxY, isPart2);
        }
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

    private static (Vector2 robotPos, int maxX, int maxY, string commands) BuildMap(List<Vector2> boxes, HashSet<Vector2> walls, bool isPart2)
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        int x = 0;
        int y = 0;
        StringBuilder sb = new();
        bool isMap = true;
        Vector2 robotPos = new();

        foreach (string line in lines)
        {
            if (line.Length == 0)
                isMap = false;
            else if (isMap)
            {
                x = 0;
                for (int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '#')
                    {
                        walls.Add(new Vector2(x, y));
                        if (isPart2)
                            walls.Add(new Vector2(++x, y));
                    }
                    else if (line[c] == 'O')
                    {
                        boxes.Add(new Vector2(x, y));
                        if (isPart2)
                            boxes.Add(new Vector2(++x, y));
                    }
                    else
                    {
                        if (line[c] == '@')
                            robotPos = new Vector2(x, y);
                        if (isPart2)
                            x++;
                    }
                    x++;
                }
                y++;
            }
            else
            {
                sb.Append(line);
            }
        }

        return (robotPos, --x, --y, sb.ToString());
    }

    private static void PrintMap(Vector2 robotPos, List<Vector2> boxes, HashSet<Vector2> walls, int maxX, int maxY, bool isPart2)
    {
        bool isFirstBox = true;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                Vector2 currentPos = new(x, y);
                if (currentPos == robotPos)
                    Console.Write('@');
                else if (boxes.Contains(currentPos))
                {
                    if (!isPart2)
                        Console.Write('O');
                    else
                    {
                        if(isFirstBox)
                        {
                            Console.Write('[');
                            isFirstBox = false;
                        }
                        else
                        {
                            Console.Write(']');
                            isFirstBox = true;
                        }
                    }
                }
                else if (walls.Contains(currentPos))
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n\n");
    }

    internal record class Box(Vector2 Left, Vector2 Right, bool IsPart2)
    {
        public Vector2 Left { get; set; } = Left;
        public Vector2 Right { get; set; } = Right;
        public bool IsPart2 { get; set; } = IsPart2;
    }
}
