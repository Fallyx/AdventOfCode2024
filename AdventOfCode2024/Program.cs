﻿using System.Diagnostics;

namespace AdventOfCode2024;
internal class Program
{
    private static readonly Stopwatch stopwatch = new();

    static void Main(string[] args)
    {
        string? input;
        if (args.Length != 0)
        {
            input = args[0];
        }
        else
        {
            Console.WriteLine("Run a day [1-25]:");
            input = Console.ReadLine();
            Console.WriteLine("-----------------");
        }

        bool success = int.TryParse(input, out int day);
        if (!success) return;

        RunDay(day);
    }

    private static void RunDay(int day)
    {
        stopwatch.Start();

        switch(day)
        {
            case 1:
                Day01.Day01.Task1and2();
                break;
            case 2:
                Day02.Day02.Task1and2();
                break;
            case 3:
                Day03.Day03.Task1and2();
                break;
            case 4:
                Day04.Day04.Task1and2();
                break;
            case 5:
                Day05.Day05.Task1and2();
                break;
            case 6:
                Day06.Day06.Task1and2();
                break;
            case 7:
                Day07.Day07.Task1and2();
                break;
            case 8:
                Day08.Day08.Task1and2();
                break;
            case 9:
                Day09.Day09.Task1and2();
                break;
            case 10:
                Day10.Day10.Task1and2();
                break;
            case 11:
                Day11.Day11.Task1and2();
                break;
            case 12:
                Day12.Day12.Task1and2();
                break;
            case 13:
                Day13.Day13.Task1and2();
                break;
            case 14:
                Day14.Day14.Task1and2();
                break;
            case 15:
                Day15.Day15.Task1and2();
                break;
            case 16:
                Day16.Day16.Task1and2();
                break;
            case 17:
                Day17.Day17.Task1and2();
                break;
            case 18:
                Day18.Day18.Task1and2();
                break;
            case 19:
                Day19.Day19.Task1and2();
                break;
            case 20:
                Day20.Day20.Task1and2();
                break;
            case 21:
                Day21.Day21.Task1and2();
                break;
            case 22:
                Day22.Day22.Task1and2();
                break;
            case 23:
                Day23.Day23.Task1and2();
                break;
            case 24:
                Day24.Day24.Task1and2();
                Day24.Day24GVIZ.Task2();
                break;
        }

        stopwatch.Stop();
        Console.WriteLine($"Day {day:D2} finished in {stopwatch.Elapsed}");
    }
}
