using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_06 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(377891, 14110788);
        }

        private (Action, (int, int, int, int))[] Instructions { get; }

        public Puzzle_2015_06(string input)
        {
            Instructions = input
               .ParseLines(line => (ParseInstruction(line), ParseCorners(line)));
        }

        protected override object Part1()
        {
            var grid = new bool[1000, 1000];

            foreach (var (instruction, (topLeftX, topLeftY, bottomRightX, bottomRightY)) in Instructions)
            {
                for (var y = topLeftY; y <= bottomRightY; y++)
                for (var x = topLeftX; x <= bottomRightX; x++)
                {
                    grid[x, y] = instruction switch
                    {
                        Action.TurnOn  => true,
                        Action.Toggle  => !grid[x, y],
                        Action.TurnOff => false,
                        _              => throw new InvalidOperationException(),
                    };
                }
            }

            var lightsOn = 0;
            for (var y = 0; y < 1000; y++)
            for (var x = 0; x < 1000; x++)
            {
                if (grid[x, y]) lightsOn++;
            }

            return lightsOn;
        }

        protected override object Part2()
        {
            var grid = new int[1000, 1000];

            foreach (var (instruction, (topLeftX, topLeftY, bottomRightX, bottomRightY)) in Instructions)
            {
                for (var y = topLeftY; y <= bottomRightY; y++)
                for (var x = topLeftX; x <= bottomRightX; x++)
                {
                    grid[x, y] = instruction switch
                    {
                        Action.TurnOn  => grid[x, y] + 1,
                        Action.Toggle  => grid[x, y] + 2,
                        Action.TurnOff => Math.Max(grid[x, y] -1, 0),
                        _              => throw new InvalidOperationException(),
                    };
                }
            }

            var totalBrightness = 0;
            for (var y = 0; y < 1000; y++)
            for (var x = 0; x < 1000; x++)
            {
                totalBrightness += grid[x, y];
            }

            return totalBrightness;
        }

        private static Action ParseInstruction(string str)
        {
            return str.StartsWith("turn on")
                ? Action.TurnOn
                : str.StartsWith("toggle")
                    ? Action.Toggle
                    : Action.TurnOff;
        }

        private static (int, int, int, int) ParseCorners(string str)
        {
            return str.Int32Matches().Parse(a => (a[0], a[1], a[2], a[3]));
        }

        private enum Action
        {
            TurnOn,
            Toggle,
            TurnOff
        }
    }
}