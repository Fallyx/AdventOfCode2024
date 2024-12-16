using System.Numerics;

namespace AdventOfCode2024.Day16;

internal class Day16
{
    private const string inputPath = @"Day16/Input.txt";
    private static readonly List<Vector2> directions =
    [
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(0, 1),
    ];

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        Path startPos = null;
        Vector2 endPos = new();
        Dictionary<string, int> paths = [];
        Dictionary<string, Path> pathMap = [];

        int y = 0;
        foreach(string line in lines)
        {
            for (int x = 0; x < line.Length; x++)
            {
                Vector2 pos = new(x, y);
                if (line[x] == '.' || line[x] == 'E')
                {
                    Path pE = new(pos, 0, int.MaxValue, []);
                    pathMap.Add(pE.GetKey(), pE);
                    Path pS = new(pos, 1, int.MaxValue, []);
                    pathMap.Add(pS.GetKey(), pS);
                    Path pW = new(pos, 2, int.MaxValue, []);
                    pathMap.Add(pW.GetKey(), pW);
                    Path pN = new(pos, 3, int.MaxValue, []);
                    pathMap.Add(pN.GetKey(), pN);
                    if (line[x] == 'E')
                    {
                        endPos = pos;
                    }
                }
                else if (line[x] == 'S')
                {
                    Path pE = new(pos, 0, 0, []);
                    pathMap.Add(pE.GetKey(), pE);
                    Path pS = new(pos, 1, int.MaxValue, []);
                    pathMap.Add(pS.GetKey(), pS);
                    Path pW = new(pos, 2, int.MaxValue, []);
                    pathMap.Add(pW.GetKey(), pW);
                    Path pN = new(pos, 3, int.MaxValue, []);
                    pathMap.Add(pN.GetKey(), pN);
                    startPos = pE;
                }
            }
            y++;
        }

        ShortestPath(pathMap, startPos, endPos);

        List<Path> endPaths = pathMap.Where(p => p.Value.Pos == endPos).Select(p => p.Value).ToList();
        int lowestScore = int.MaxValue;
        HashSet<string> bestSpotKeys = [];

        foreach(Path p in endPaths)
        {
            if (p.Score < lowestScore)
            {
                bestSpotKeys = [];
                lowestScore = p.Score;
                foreach (Path bS in p.Paths)
                {
                    bestSpotKeys.Add(bS.GetCoord());
                }
            }
        }

        Console.WriteLine($"Task 1: {lowestScore}");
        Console.WriteLine($"Task 2: {bestSpotKeys.Count + 1}");
    }

    private static void ShortestPath(Dictionary<string, Path> pathMap, Path start, Vector2 endPos)
    {
        Queue<Path> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Path p = queue.Dequeue();

            if (p.Pos == endPos)
                continue;

            Vector2 forwardPos = p.Pos + directions[p.DirIdx];
            String forwardKey = $"{forwardPos.X}-{forwardPos.Y}-{p.DirIdx}";
            int score = p.Score + 1;
            UpdatePath(forwardKey, score, p, pathMap, queue);

            int leftIdx = ((p.DirIdx - 1) % 4 + 4) % 4;
            String turnLeftKey = $"{p.Pos.X}-{p.Pos.Y}-{leftIdx}";
            score = p.Score + 1000;
            UpdatePath(turnLeftKey, score, p, pathMap, queue);

            int rightIdx = (p.DirIdx + 1) % 4;
            String turnRightKey = $"{p.Pos.X}-{p.Pos.Y}-{rightIdx}";
            UpdatePath(turnRightKey, score, p, pathMap, queue);
        }
    }

    private static void UpdatePath(string key, int score, Path p, Dictionary<string, Path> pathMap, Queue<Path> queue)
    {
        if (pathMap.TryGetValue(key, out Path nextPath))
        {
            if (score < nextPath.Score)
            {
                nextPath.Score = score;
                nextPath.Paths = [.. p.Paths];
                nextPath.Paths.Add(p);
                queue.Enqueue(nextPath);
            }
            else if (score == nextPath.Score)
            {
                nextPath.Paths.AddRange(p.Paths);
                nextPath.Paths.Add(p);
            }
        }
    }

    internal record Path(Vector2 Pos, int DirIdx, Int32 Score, List<Path> Paths)
    {
        public Int32 Score { get; set; } = Score;
        public List<Path> Paths { get; set; } = Paths;

        public string GetKey()
        {
            return $"{Pos.X}-{Pos.Y}-{DirIdx}";
        }

        public string GetCoord()
        {
            return $"{Pos.X}-{Pos.Y}";
        }
    }
}
