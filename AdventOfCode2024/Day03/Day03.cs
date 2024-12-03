using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day03;

internal partial class Day03
{
    const string inputPath = @"Day03/Input.txt";

    [GeneratedRegex(@"(mul\((\d+),(\d+)\)|(don't|do))")]
    private static partial Regex MulInstrRegex();

    internal static void Task1and2()
    {
        long multsP1 = 0;
        long multsP2 = 0;
        bool canMult = true;

        foreach(string line in File.ReadLines(inputPath))
        {
            MatchCollection mc = MulInstrRegex().Matches(line);

            foreach(Match m in mc)
            {
                if (m.Groups[0].ToString() == "do")
                    canMult = true;
                else if (m.Groups[0].ToString() == "don't")
                    canMult = false;
                else if (canMult)
                {
                    multsP1 += int.Parse(m.Groups[2].ToString()) * int.Parse(m.Groups[3].ToString());
                    multsP2 += int.Parse(m.Groups[2].ToString()) * int.Parse(m.Groups[3].ToString());
                }
                else
                    multsP1 += int.Parse(m.Groups[2].ToString()) * int.Parse(m.Groups[3].ToString());
            }
        }

        Console.WriteLine($"Task 1: {multsP1}");
        Console.WriteLine($"Task 2: {multsP2}");
    }
}
