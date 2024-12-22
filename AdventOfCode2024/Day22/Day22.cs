namespace AdventOfCode2024.Day22;

internal class Day22
{
    private const string inputPath = @"Day22/Input.txt";

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        long totSecretNums = 0;
        List<int>[] buyers = new List<int>[lines.Count()];

        int buyerIdx = 0;
        foreach(string line in lines)
        {
            long secretNum = int.Parse(line);
            buyers[buyerIdx] = [(int)(secretNum % 10)];

            for (int i = 0; i < 2000; i++)
            {
                secretNum = MixAndPrune(secretNum * 64, secretNum);
                secretNum = MixAndPrune(secretNum / 32, secretNum);
                secretNum = MixAndPrune(secretNum * 2048, secretNum);
                buyers[buyerIdx].Add((int)(secretNum % 10));
            }

            totSecretNums += secretNum;
            buyerIdx++;
        }

        Console.WriteLine($"Task 1: {totSecretNums}");

        Dictionary<string, int> mostBananasBySeq = [];

        for (int i = 0; i < buyers.Length; i++)
        {
            HashSet<string> sequenceOccured = [];
            for(int x = 0; x < buyers[i].Count - 4; x++)
            {
                List<int> sublist = buyers[i].Skip(x).Take(5).ToList();
                int[] priceChange = new int[4];
                for (int y = 0; y < sublist.Count - 1; y++)
                {
                    priceChange[y] = sublist[y + 1] - sublist[y];
                }

                string key = string.Join(',', priceChange);

                if (sequenceOccured.Contains(key))
                    continue;

                if (mostBananasBySeq.ContainsKey(key))
                    mostBananasBySeq[key] += sublist.Last();
                else
                    mostBananasBySeq.Add(key, sublist.Last());

                sequenceOccured.Add(key);
            }
        }

        Console.WriteLine($"Task 2: {mostBananasBySeq.Select(b => b.Value).Max()}");
    }

    internal static long MixAndPrune(long result, long secretNum)
    {
        secretNum ^= result;
        secretNum %= 16777216;

        return secretNum;
    }
}
