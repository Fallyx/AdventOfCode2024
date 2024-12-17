namespace AdventOfCode2024.Day17;

internal class Day17
{
    private const string inputPath = @"Day17/Input.txt";

    internal static void Task1and2()
    {
        long a = 0;
        long b = 0;
        long c = 0;

        List<int> program = [];

        foreach (string line in File.ReadLines(inputPath))
        {
            if (line.StartsWith("Register A:"))
                a = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Register B:"))
                b = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Register C:"))
                c = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Program:"))
                program = line.Split([':', ' ', ','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
        }

        List<int> outputs = RunProgram(a, b, c, program);
        Console.WriteLine($"Task 1: {string.Join(",", outputs)}");

        long minA = RunProgram(program);
        Console.WriteLine($"Task 2: {minA}");
    }

    private static List<int> RunProgram(long a, long b, long c, List<int> program)
    {
        List<int> outputs = [];
        int idx = 0;

        while (idx < program.Count)
        {
            int opCode = program[idx];
            int operand = program[idx + 1];
            long comboOperand = GetComboOperand(operand, a, b, c);

            if (opCode == 0)
                a /= (long)Math.Pow(2, comboOperand);
            else if (opCode == 1)
                b ^= operand;
            else if (opCode == 2)
                b = comboOperand % 8;
            else if (opCode == 3 && a != 0)
            {
                idx = operand;
                idx -= 2;
            }
            else if (opCode == 4)
                b ^= c;
            else if (opCode == 5)
                outputs.Add((int)(comboOperand % 8));
            else if (opCode == 6)
                b = a / (long)Math.Pow(2, comboOperand);
            else if (opCode == 7)
                c = a / (long)Math.Pow(2, comboOperand);

            idx += 2;
        }

        return outputs;
    }

    private static long RunProgram(List<int> program)
    {
        Queue<long> queue = new([1, 2, 3, 4, 5, 6, 7, 8]);
        long minA = long.MaxValue;

        while (queue.Count > 0)
        {
            long a = queue.Dequeue();
            long b = 0;
            long c = 0;

            List<int> sameOutputs = sameOutputs = RunProgram(a, b, c, program);

            if (sameOutputs.Count > program.Count)
                continue;

            bool isSubset = true;
            for (int x = 0; x < sameOutputs.Count; x++)
            {
                if (sameOutputs[sameOutputs.Count - 1 - x] != program[program.Count - 1 - x])
                {
                    isSubset = false;
                    break;
                }
            }

            if (isSubset && sameOutputs.Count < program.Count)
            {
                a *= 8;
                for (int i = 0; i < 8; i++)
                {
                    queue.Enqueue(a + i);
                }
            }
            else if (isSubset && sameOutputs.Count == program.Count && minA > a)
                    minA = a;
        }

        return minA;
    }

    private static long GetComboOperand(int comboOperand, long a, long b, long c) => comboOperand switch
    {
        >= 0 and <= 3 => comboOperand,
        4 => a,
        5 => b,
        6 => c,
        7 => throw new Exception("ERROR")
    };
}
