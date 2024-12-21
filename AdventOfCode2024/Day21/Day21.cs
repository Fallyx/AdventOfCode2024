using System.Numerics;

namespace AdventOfCode2024.Day21;

internal class Day21
{
    private const string inputPath = @"Day21/Input.txt";
    private static readonly Dictionary<Vector2, char> directions = new()
    {
        { new(1, 0), '>' },
        { new(-1, 0), '<' },
        { new(0, 1), 'v' },
        { new(0, -1), '^' }
    };

    internal static void Task1and2()
    {
        Dictionary<char, Dictionary<char, List<string>>> numericKeypadLookup = GetKeypadTable(GetNumericKeypadCoords(), new(0, 3));
        Dictionary<char, Dictionary<char, List<string>>> directionKeypadLookup = GetKeypadTable(GetDirectionalKeypadCoords(), new(0, 0));
        long shortestPathP1 = 0;
        long shortestPathP2 = 0;

        foreach (string line in File.ReadLines(inputPath))
        {
            int num = int.Parse(line[..^1]);
            shortestPathP1 += num * ShortestSequence(line, numericKeypadLookup, directionKeypadLookup, [], 3, true);
            shortestPathP2 += num * ShortestSequence(line, numericKeypadLookup, directionKeypadLookup, [], 26, true);
        }
        Console.WriteLine($"Task 1: {shortestPathP1}");
        Console.WriteLine($"Task 1: {shortestPathP2}");
    }

    private static long ShortestSequence(string code, Dictionary<char, Dictionary<char, List<string>>> numericKeypadLookup, Dictionary<char, Dictionary<char, List<string>>> directionKeypadLookup, Dictionary<string, long> memo, int depth, bool isNumeric)
    {
        if (depth == 0)
            return code.Length + 1;

        long shortestPath = 0;
        char currentChar = 'A';
        Dictionary<char, Dictionary<char, List<string>>> lookup = (isNumeric ? numericKeypadLookup : directionKeypadLookup);
        code = (isNumeric ? code : code + "A");

        foreach (char c in code)
        {
            if (c == currentChar)
            {
                shortestPath++;
                continue;
            }

            long cShortestPath = long.MaxValue;
            foreach (String path in lookup[currentChar][c])
            {
                string key = $"{path}-{depth}";
                if (memo.TryGetValue(key, out long value))
                    cShortestPath = Math.Min(cShortestPath, value);
                else
                {
                    long result = ShortestSequence(path, numericKeypadLookup, directionKeypadLookup, memo, depth - 1, false);
                    memo.Add(key, result);
                    cShortestPath = Math.Min(cShortestPath, result);
                }
            }

            shortestPath += cShortestPath;
            currentChar = c;
        }

        return shortestPath;
    }

    /*
    +---+---+---+
    | 7 | 8 | 9 |
    +---+---+---+
    | 4 | 5 | 6 |
    +---+---+---+
    | 1 | 2 | 3 |
    +---+---+---+
        | 0 | A |
        +---+---+
    */
    private static Dictionary<Vector2, char> GetNumericKeypadCoords()
    {
        return new()
        {
            { new(0, 0), '7' },
            { new(1, 0), '8' },
            { new(2, 0), '9' },
            { new(0, 1), '4' },
            { new(1, 1), '5' },
            { new(2, 1), '6' },
            { new(0, 2), '1' },
            { new(1, 2), '2' },
            { new(2, 2), '3' },
            { new(1, 3), '0' },
            { new(2, 3), 'A' }
        };
    }

    /*
        +---+---+
        | ^ | A |
    +---+---+---+
    | < | v | > |
    +---+---+---+
    */
    private static Dictionary<Vector2, char> GetDirectionalKeypadCoords()
    {
        return new()
        {
            { new(1, 0), '^' },
            { new(2, 0), 'A' },
            { new(0, 1), '<' },
            { new(1, 1), 'v' },
            { new(2, 1), '>' }
        };
    }

    private static Dictionary<char, Dictionary<char, List<string>>> GetKeypadTable(Dictionary<Vector2, char> coords, Vector2 forbiddenNode)
    {
        Dictionary<char, Dictionary<char, List<string>>> tables = [];

        foreach(KeyValuePair<Vector2, char> coord in coords)
        {
            Dictionary<char, List<string>> paths = GetPaths(coord.Key, forbiddenNode, coords);
            tables.Add(coord.Value, paths);
        }

        return tables;
    }

    private static Dictionary<char, List<string>> GetPaths(Vector2 startPos, Vector2 forbiddenNode, Dictionary<Vector2, char> coords)
    {
        Dictionary<char, List<string>> paths = [];
        Stack<(Vector2 pos, String path)> stack = new();
        stack.Push((startPos, ""));

        while(stack.Count > 0)
        {
            (Vector2 currentPos, String path) = stack.Pop();

            foreach (KeyValuePair<Vector2, char> dir in directions)
            {
                Vector2 nextPos = currentPos + dir.Key;
                if (nextPos == forbiddenNode || nextPos == startPos || !coords.ContainsKey(nextPos))
                    continue;

                if (path.Length > (int)(Math.Abs(startPos.X - nextPos.X) + Math.Abs(startPos.Y - nextPos.Y)))
                    continue;

                String newPath = path + dir.Value;

                if (paths.TryGetValue(coords[nextPos], out List<string> value))
                    value.Add(newPath);
                else
                    paths.Add(coords[nextPos], [newPath]);

                stack.Push((nextPos, newPath));
            }
        }

        return paths;
    }
}
