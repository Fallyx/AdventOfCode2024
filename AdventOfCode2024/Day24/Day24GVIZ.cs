using System.Text;

namespace AdventOfCode2024.Day24
{
    internal class Day24GVIZ
    {
        private const string inputPath = @"Day24/InputGviz.txt";

        internal static void Task2()
        {
            StringBuilder gviz = new();
            gviz.AppendLine("rankdir=\"TB\"");

            foreach (string line in File.ReadLines(inputPath))
            {
                string[] logicGates = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                gviz.Append(logicGates[0]).Append(" -> ").Append(logicGates[1]).Append('_').Append(logicGates[0]).Append(logicGates[2]).AppendLine(";");
                gviz.Append(logicGates[2]).Append(" -> ").Append(logicGates[1]).Append('_').Append(logicGates[0]).Append(logicGates[2]).AppendLine(";");
                gviz.Append(logicGates[1]).Append('_').Append(logicGates[0]).Append(logicGates[2]).Append(" -> ").Append(logicGates[4]).AppendLine(";");
                gviz.Append(logicGates[1]).Append('_').Append(logicGates[0]).Append(logicGates[2]).Append("[label=").Append(logicGates[1]).AppendLine("]\n");
            }

            File.WriteAllText(@"Day24/OutputGviz.txt", gviz.ToString());

            /*
                To swap:
                z08 -> cdj
                z16 -> mrb
                z32 -> gfm
                dmh -> qjd
            */
        }
    }
}
