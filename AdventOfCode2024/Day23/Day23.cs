namespace AdventOfCode2024.Day23;

internal class Day23
{
    private const string inputPath = @"Day23/Input.txt";

    internal static void Task1and2()
    {
        Dictionary<string, List<string>> connections = [];

        foreach (string line in File.ReadLines(inputPath))
        {
            string[] pcs = line.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (!connections.TryAdd(pcs[0], [pcs[1]]))
                connections[pcs[0]].Add(pcs[1]);

            if (!connections.TryAdd(pcs[1], [pcs[0]]))
                connections[pcs[1]].Add(pcs[0]);
        }

        int threeInterconnectedPcs = Part1(connections);
        Console.WriteLine($"Task 1: {threeInterconnectedPcs}");

        List<string> pcList = [.. Part2(connections)];
        pcList.Sort();
        Console.WriteLine($"Task 2: {string.Join(',', pcList)}");
    }

    internal static int Part1(Dictionary<string, List<string>> connections)
    {
        int threeInterconnectedPcs = 0;
        HashSet<string> seen = [];

        foreach (KeyValuePair<string, List<string>> connection in connections)
        {
            if (!connection.Key.StartsWith('t'))
                continue;

            foreach (string pc in connection.Value)
            {
                List<string> threes = connections[pc].Where(connection.Value.Contains).ToList();

                foreach (string three in threes)
                {
                    List<string> list = [connection.Key, pc, three];
                    list.Sort();
                    string key = string.Join(',', list);

                    if (seen.Contains(key))
                        continue;

                    threeInterconnectedPcs++;
                    seen.Add(key);
                }
            }
        }

        return threeInterconnectedPcs;
    }

    internal static HashSet<string> Part2(Dictionary<string, List<string>> connections)
    {
        HashSet<string> maxPcs = [];

        foreach (KeyValuePair<string, List<string>> connection in connections)
        {
            HashSet<string> pcs = [];
            pcs.Add(connection.Key);

            foreach (string c in connection.Value)
            {
                if (pcs.All(p => connections[p].Contains(c)))
                    pcs.Add(c);
            }

            if (maxPcs.Count < pcs.Count)
                maxPcs = pcs;
        }

        return maxPcs;
    }
}
