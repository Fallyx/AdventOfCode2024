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
    private static HashSet<int> idxs = [];

    internal static void Task1and2()
    {
        Task1();
        Task2();
    }

    private static void Task1()
    {
        HashSet<Vector2> walls = [];
        List<Box> boxes = [];
        (Vector2 robotPos, int maxX, int maxY, string commands) = BuildMap(boxes, walls, false);

        RunCommandsP1(commands, robotPos, boxes, walls);

        int gpsSum = 0;

        foreach (Box box in boxes)
        {
            gpsSum += (int)(box.Left.X + box.Left.Y * 100);
        }

        Console.WriteLine($"Task 1: {gpsSum}");
    }

    private static void Task2()
    {
        HashSet<Vector2> walls = [];
        List<Box> boxes = [];
        (Vector2 robotPos, int maxX, int maxY, string commands) = BuildMap(boxes, walls, true);

        RunCommandsP2(commands, robotPos, boxes, walls, maxX, maxY, false);

        int gpsSum = 0;

        foreach (Box box in boxes)
        {
            gpsSum += (int)(box.Left.X + box.Left.Y * 100);
        }

        Console.WriteLine($"Task 2: {gpsSum}");
    }

    private static void RunCommandsP1(string commands, Vector2 robotPos, List<Box> boxes, HashSet<Vector2> walls, int maxX = 0, int maxY = 0, bool printMap = false)
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
            if (CanMoveP1(nextPos, dir, boxes, walls))
                robotPos = nextPos;

            if (printMap)
                PrintMap(robotPos, boxes, walls, maxX, maxY, false);
        }
    }

    private static void RunCommandsP2(string commands, Vector2 robotPos, List<Box> boxes, HashSet<Vector2> walls, int maxX = 0, int maxY = 0, bool printMap = false)
    {
        foreach (char c in commands)
        {
            bool isVert = false;
            Vector2 dir = new();
            if (c == '^')
            {
                dir = dirs[0];
                isVert = true;
            }
            else if (c == '>')
                dir = dirs[3];
            else if (c == 'v')
            {
                dir = dirs[1];
                isVert = true;
            }
            else if (c == '<')
                dir = dirs[2];

            if (printMap)
                Console.WriteLine($"command={c}, dir={dir}");

            Vector2 nextPos = robotPos + dir;
            if ((isVert && CanMoveVerticalP2(nextPos, dir, boxes, walls)) || (!isVert && CanMoveHorizontalP2(nextPos, dir, boxes, walls)))
            {
                robotPos = nextPos;
                foreach(int idx in idxs)
                {
                    boxes[idx].Left += dir;
                    boxes[idx].Right += dir;
                }
            }

            idxs = [];

            
            if (printMap)
                PrintMap(robotPos, boxes, walls, maxX, maxY, true);
            
        }

        if (printMap)
            PrintMap(robotPos, boxes, walls, maxX, maxY, true);
    }

    private static bool CanMoveP1(Vector2 nextPos, Vector2 dir, List<Box> boxes, HashSet<Vector2> walls)
    {
        if (walls.Contains(nextPos))
            return false;

        if (!boxes.Select(b => b.Left).Contains(nextPos))
            return true;

        Box tmp = new(nextPos, new(-1));
        int idx = boxes.IndexOf(tmp);
        if (CanMoveP1(nextPos + dir, dir, boxes, walls))
        {
            tmp.Left = new(nextPos.X + dir.X, nextPos.Y + dir.Y);
            boxes[idx] = tmp;
            return true;
        }
        else
            return false;
    }

    private static bool CanMoveHorizontalP2(Vector2 nextPos, Vector2 dir, List<Box> boxes, HashSet<Vector2> walls)
    {
        if (walls.Contains(nextPos))
            return false;

        Box? b = (dir == dirs[2] ? boxes.Find(b => b.Right == nextPos) : boxes.Find(b => b.Left == nextPos));
        if (b == null)
            return true;

        int idx = boxes.IndexOf(b);

        if (CanMoveHorizontalP2(nextPos + dir * 2, dir, boxes, walls))
        {
            idxs.Add(idx);
            return true;
        }

        return false;
    }

    private static bool CanMoveVerticalP2(Vector2 nextPos, Vector2 dir, List<Box> boxes, HashSet<Vector2> walls)
    {
        if (walls.Contains(nextPos))
            return false;

        Box? b = boxes.Find(b => b.Left == nextPos);
        if (b == null)
        {
            b = boxes.Find(b => b.Right == nextPos);
            if (b == null)
                return true;
        }

        int idx = boxes.IndexOf(b);
        if (CanMoveVerticalP2(b.Left + dir, dir, boxes, walls) && CanMoveVerticalP2(b.Right + dir, dir, boxes, walls))
        {
            idxs.Add(idx);
            return true;
        }

        return false;
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

    private static (Vector2 robotPos, int maxX, int maxY, string commands) BuildMap(List<Box> boxes, HashSet<Vector2> walls, bool isPart2)
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
                        Vector2 left = new Vector2(x, y);
                        Vector2 right = (isPart2 ? new Vector2(++x, y) : new(-1, -1));
                        boxes.Add(new(left, right));
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

    private static void PrintMap(Vector2 robotPos, List<Box> boxes, HashSet<Vector2> walls, int maxX, int maxY, bool isPart2)
    {
        Console.Clear();
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                Vector2 currentPos = new(x, y);
                if (currentPos == robotPos)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('@');
                    Console.ResetColor();
                }
                else if (boxes.Select(b => b.Left).Contains(currentPos))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (!isPart2)
                        Console.Write('O');
                    else
                        Console.Write('[');
                    Console.ResetColor();
                }
                else if (boxes.Select(b => b.Right).Contains(currentPos))
                    Console.Write(']');
                else if (walls.Contains(currentPos))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write('#');
                    Console.ResetColor();
                }
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n\n");
        Thread.Sleep(500);
    }

    internal record Box(Vector2 Left, Vector2 Right)
    {
        public Vector2 Left { get; set; } = Left;
        public Vector2 Right { get; set; } = Right;
    }
}
