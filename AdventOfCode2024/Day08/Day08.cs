using System.Numerics;

namespace AdventOfCode2024.Day08;

internal class Day08
{
    private const string inputPath = @"Day08/Input.txt";
    private static int xMax;
    private static int yMax;
    private static readonly HashSet<Vector2> antinodesP1 = [];
    private static readonly HashSet<Vector2> antinodesP2 = [];

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        Dictionary<char, List<Vector2>> antennas = [];

        xMax = lines.First().Length - 1;
        yMax = lines.Count() - 1;

        int y = 0;
        foreach(string line in lines)
        {
            for(int x = 0; x < line.Length; x++)
            {
                if (line[x] == '.')
                    continue;

                Vector2 pos = new(x, y);
                if (!antennas.TryAdd(line[x], [pos]))
                {
                    antennas[line[x]].Add(pos);
                }
            }
            y++;
        }

        foreach(KeyValuePair<char, List<Vector2>> entry in antennas)
        {
            for (int i = 0; i < entry.Value.Count - 1; i++)
            {
                for (int x = i + 1; x < entry.Value.Count; x++)
                {
                    Vector2 antenna1 = entry.Value[i];
                    Vector2 antenna2 = entry.Value[x];

                    GetAntinodePos(antenna1, antenna2);
                    GetAntinodePos(antenna2, antenna1);
                }
            }
        }

        Console.WriteLine($"Task 1: {antinodesP1.Count}");
        Console.WriteLine($"Task 2: {antinodesP2.Count}");
    }

    private static void GetAntinodePos(Vector2 antenna1, Vector2 antenna2)
    {
        Vector2 direction = antenna1 - antenna2;
        Vector2 antinodePos = antenna1 + direction;
        bool task1 = true;
        antinodesP2.Add(antenna1);
        antinodesP2.Add(antenna2);

        while (antinodePos.X >= 0 && antinodePos.X <= xMax && antinodePos.Y >= 0 && antinodePos.Y <= yMax)
        {
            if (task1)
            {
                antinodesP1.Add(antinodePos);
                task1 = false;
            }

            antinodesP2.Add(antinodePos);
            antinodePos += direction;
        }
    }
}
