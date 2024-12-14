using System.Numerics;

namespace AdventOfCode2024.Day14;

internal class Day14
{
    private const string inputPath = @"Day14/Input.txt";
    private const int xMax = 101;
    private const int yMax = 103;

    internal static void Task1and2()
    {
        List<Robot> robots = [];

        foreach (string line in File.ReadLines(inputPath))
        {
            string[] inputs = line.Split(['=', ',', ' '], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Vector2 pos = new(int.Parse(inputs[1]), int.Parse(inputs[2]));
            Vector2 vel = new(int.Parse(inputs[4]), int.Parse(inputs[5]));
            robots.Add(new(pos, vel));
        }

        int seconds = 1;
        while(true)
        {
            foreach (Robot rob in robots)
            {
                float newX = (rob.Pos.X + rob.Vel.X) % xMax;
                newX = (newX < 0 ? xMax + newX : newX);
                float newY = (rob.Pos.Y + rob.Vel.Y) % yMax;
                newY = (newY < 0 ? yMax + newY : newY);
                rob.Pos = new(newX, newY);
            }

            if (seconds == 100)
                CalcP1(robots);

            if (robots.Select(r => r.Pos).Distinct().Count() == robots.Count)
                break;
            
            seconds++;
        }

        Console.WriteLine($"Task 2: {seconds}");
    }

    private static void CalcP1(List<Robot> robots)
    {
        int midX = xMax / 2;
        int midY = yMax / 2;
        int[] quadrants = new int[4]; // 0 upper left, 1 upper right, 2 lower left, 3 lower right
        foreach (Robot rob in robots)
        {
            if (rob.Pos.Y < midY)
            {
                if (rob.Pos.X < midX)
                    quadrants[0]++;
                else if (rob.Pos.X > midX)
                    quadrants[1]++;
            }
            else if (rob.Pos.Y > midY)
            {
                if (rob.Pos.X < midX)
                    quadrants[2]++;
                else if (rob.Pos.X > midX)
                    quadrants[3]++;
            }
        }

        Console.WriteLine($"Task 1: {quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]}");
    }

    internal class Robot(Vector2 pos, Vector2 vel)
    {
        public Vector2 Pos { get; set; } = pos;
        public Vector2 Vel { get; set; } = vel;
    }
}
