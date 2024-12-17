namespace AdventOfCode2024.Day17;

internal class Day17
{
    private const string inputPath = @"Day17/Input.txt";

    internal static void Task1and2()
    {
        long a = 0;
        long b = 0;
        long c = 0;

        int[] program = null;
        List<int> outputs = [];

        foreach (string line in File.ReadLines(inputPath))
        {
            if (line.StartsWith("Register A:"))
                a = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Register B:"))
                b = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Register C:"))
                c = long.Parse(line.Split(' ')[2]);
            else if (line.StartsWith("Program:"))
                program = line.Split([':', ' ', ','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        }

        int idx = 0;
        while(idx < program.Length)
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

        Console.WriteLine($"Task 1: {string.Join(",", outputs)}");        

        for (long a2 = 0; a2 < long.MaxValue; a2++)
        {
            idx = 0;
            a = a2;
            b = 0;
            c = 0;
            outputs = [];
            while (idx < program.Length)
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

            if (outputs.Count == program.Length)
            {
                bool found = true;
                for (int i = 0; i < program.Length; i++)
                {
                    if (program[i] != outputs[i])
                    {
                        found = false;
                        break;
                    }

                }
                if (found)
                {
                    Console.WriteLine($"Task 2: {a2}");
                    break;
                }
            }
        }
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
