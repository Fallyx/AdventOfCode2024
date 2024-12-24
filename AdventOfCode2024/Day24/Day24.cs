using System.Text;

namespace AdventOfCode2024.Day24;

internal class Day24
{
    private const string inputPath = @"Day24/Input.txt";

    internal static void Task1and2()
    {
        IEnumerable<string> lines = File.ReadLines(inputPath);
        Dictionary<string, bool> wireValues = [];
        bool isGateLogic = false;
        Queue<BooleanLogicGate> gateLogicLineQueue = [];

        foreach(string line in lines)
        {
            if (line.Length == 0)
            {
                isGateLogic = true;
                continue;
            }
                
            if (!isGateLogic)
            {
                string[] input = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                wireValues.Add(input[0], (input[1] == "1"));
            }
            else
            {
                string[] logicGates = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                gateLogicLineQueue.Enqueue(new(logicGates[0], logicGates[2], logicGates[1], logicGates[4]));
            }      
        }

        while (gateLogicLineQueue.Count > 0)
        {
            BooleanLogicGate wires = gateLogicLineQueue.Dequeue();

            if (!wireValues.ContainsKey(wires.InputLeft) || !wireValues.ContainsKey(wires.InputRight))
            {
                gateLogicLineQueue.Enqueue(wires);
                continue;
            }

            bool left = wireValues[wires.InputLeft];
            bool right = wireValues[wires.InputRight];
            bool result;

            if (wires.Operation == "AND")
                result = (left == true && right == true);
            else if (wires.Operation == "OR")
                result = (left || right);
            else if (wires.Operation == "XOR")
                result = (left != right);
            else
                throw new Exception();

            if (!wireValues.TryAdd(wires.Output, result))
                wireValues[wires.Output] = result;
        }

        bool[] zValues = wireValues.Where(w => w.Key.StartsWith('z')).OrderByDescending(w => w.Key).Select(w => w.Value).ToArray();

        StringBuilder sb = new();
        for (int i = 0; i < zValues.Length; i++)
        {
            sb.Append((zValues[i] ? "1" : "0"));
        }

        Console.WriteLine($"Task 1: {Convert.ToInt64(sb.ToString(), 2)}");
        Console.WriteLine($"Task 2: cdj,dhm,gfm,mrb,qjd,z08,z16,z32"); // Solved with Graphviz
    }

    internal record BooleanLogicGate(string InputLeft, string InputRight, string Operation, string Output);
}
