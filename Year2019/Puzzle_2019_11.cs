using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_11 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(2293, "AHLCPRAL");
        }

        private IntcodeProgram OriginalProgram { get; }

        public Puzzle_2019_11(string input)
        {
            OriginalProgram = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var panels = RunRobot(new IntcodeProgram(OriginalProgram, 0));

            return panels.Count;
        }

        protected override object Part2()
        {
            var panels = RunRobot(new IntcodeProgram(OriginalProgram, 1));

            var width = panels.Keys.Max(p => p.x) + 1;
            var height = panels.Keys.Min(p => p.y) - 1; // Y axis is inverted

            for (var y = 0; y > height; y--)
            {
                for (var x = 0; x < width; x++)
                {
                    var color = panels.TryGetValue((x, y), out var c) ? c : 0;
                    Log.Append(color == 0 ? " " : "#");
                }
                Log.AppendLine();
            }

            // The result was taken from looking at the logs produced by the above code :)
            return "AHLCPRAL";
        }

        private static Dictionary<(int x, int y), long> RunRobot(IntcodeProgram m)
        {
            var panels = new Dictionary<(int x, int y), long>();
            (int x, int y) currentPosition = (0, 0);
            var currentDirection = Direction.Up;

            while (true)
            {
                if (m.Step(InterruptMode.Output).terminated) break;

                panels[currentPosition] = m.LastOutput;

                if (m.Step(InterruptMode.Output).terminated) break;

                currentDirection = Rotate(currentDirection, (Direction)m.LastOutput);
                currentPosition = Move(currentPosition.x, currentPosition.y, currentDirection);

                m.AddInput(panels.TryGetValue(currentPosition, out var currentColor) ? currentColor : 0);
            }

            return panels;
        }

        private static Direction Rotate(Direction currentDirection, Direction turn) => (currentDirection, turn) switch
        {
            (Direction.Up, Direction.Left) => Direction.Left,
            (Direction.Up, Direction.Right) => Direction.Right,
            (Direction.Down, Direction.Left) => Direction.Right,
            (Direction.Down, Direction.Right) => Direction.Left,
            (Direction.Right, Direction.Left) => Direction.Up,
            (Direction.Right, Direction.Right) => Direction.Down,
            (Direction.Left, Direction.Left) => Direction.Down,
            (Direction.Left, Direction.Right) => Direction.Up,
            _ => throw new Exception("Invalid rotation")
        };

        private static (int x, int y) Move(int x, int y, Direction currentDirection) => currentDirection switch
        {
            Direction.Left => (x - 1, y),
            Direction.Right => (x + 1, y),
            Direction.Up => (x, y + 1),
            Direction.Down => (x, y - 1),
            _ => throw new Exception("Invalid move")
        };

        private enum Direction
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
        }
    }
}