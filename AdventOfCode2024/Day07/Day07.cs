namespace AdventOfCode2024.Day07;

internal class Day07
{
    private const string inputPath = @"Day07/Input.txt";
    internal static void Task1and2()
    {
        long totalCalibrationResultP1 = 0;
        long totalCalibrationResultP2 = 0;

        foreach(string line in File.ReadLines(inputPath))
        {
            long[] equation = line.Split([' ', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            if (CalculateTask1(equation[0], equation.Skip(1).ToArray()))
                totalCalibrationResultP1 += equation[0];
            if (CalculateTask2(equation[0], equation.Skip(1).ToArray()))
                totalCalibrationResultP2 += equation[0];
        }

        Console.WriteLine($"Task 1: {totalCalibrationResultP1}");
        Console.WriteLine($"Task 2: {totalCalibrationResultP2}");
    }

    private static bool CalculateTask1(long result, long[] nums)
    {
        if (nums.Length == 1)
        {
            if (result == nums[0])
                return true;
        }
        else
        {
            long add = nums[0] + nums[1];
            long mult = nums[0] * nums[1];
            if (result >= add)
            {
                if (CalculateTask1(result, [add, .. nums.Skip(2).ToArray()]))
                    return true;
            }
            if (result >= mult)
            {
                if (CalculateTask1(result, [mult, .. nums.Skip(2).ToArray()]))
                    return true;
            }
        }
        return false;
    }

    private static bool CalculateTask2(long result, long[] nums)
    {
        if (nums.Length == 1)
        {
            if (result == nums[0])
                return true;
        }
        else
        {
            long add = nums[0] + nums[1];
            long mult = nums[0] * nums[1];
            long concat = long.Parse(nums[0].ToString() + nums[1].ToString());
            if (result >= add)
            {
                if (CalculateTask2(result, [add, .. nums.Skip(2).ToArray()]))
                    return true;
            }
            if (result >= mult)
            {
                if (CalculateTask2(result, [mult, .. nums.Skip(2).ToArray()]))
                    return true;
            }
            if (result >= concat)
            {
                if (CalculateTask2(result, [concat, .. nums.Skip(2).ToArray()]))
                    return true;
            }
        }
        return false;
    }
}
