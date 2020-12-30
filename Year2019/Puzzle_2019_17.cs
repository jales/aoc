using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_17 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(4044, 893283);
        }

        private IntcodeProgram OriginalProgram { get; }
        private State InitialState { get; }

        public Puzzle_2019_17(string input)
        {
            OriginalProgram = new IntcodeProgram(input);

            InitialState = GetInitialState();
        }

        protected override object Part1()
        {
            var sum = 0;

            foreach (var (x, y) in InitialState.Path)
            {
                if (InitialState.Path.Contains((x + 1, y)) &&
                    InitialState.Path.Contains((x - 1, y)) &&
                    InitialState.Path.Contains((x, y + 1)) &&
                    InitialState.Path.Contains((x, y - 1)))
                {
                    sum += x * y;
                }
            }

            return sum;
        }

        protected override object Part2()
        {
            var directions = GetDirections(InitialState);

            var functions = SplitDirectionsIntoFunctions(directions);

            var p = new IntcodeProgram(OriginalProgram);

            p.Memory.WriteTo(0, 2);

            foreach (var c in functions) p.AddInput(c);

            p.Run();

            return p.Outputs.Last();
        }

        private State GetInitialState()
        {
            var p = new IntcodeProgram(OriginalProgram);
            p.Run();

            var state = new State();

            var x = 0;
            var y = 0;

            foreach (var output in p.Outputs)
            {
                switch ((char)output)
                {
                    case '#':
                        state.Path.Add((x, y));
                        break;
                    case '^':
                        state.RobotPosition = (x, y);
                        state.RobotDirection = Direction.Up;
                        break;
                    case '>':
                        state.RobotPosition = (x, y);
                        state.RobotDirection = Direction.Right;
                        break;
                    case 'v':
                        state.RobotPosition = (x, y);
                        state.RobotDirection = Direction.Down;
                        break;
                    case '<':
                        state.RobotPosition = (x, y);
                        state.RobotDirection = Direction.Left;
                        break;
                }

                if (output == 10)
                {
                    y++;
                    x = 0;
                }
                else
                {
                    x++;
                }
            }

            return state;
        }

        private static string GetDirections(State initialState)
        {
            var currentPosition = initialState.RobotPosition;
            var currentDirection = initialState.RobotDirection;
            var directions = "";

            while (TryGetNextDirection(initialState, currentDirection, currentPosition, out var nextDirection))
            {
                directions += GetTurn(currentDirection, nextDirection);
                currentDirection = nextDirection;

                int steps;
                (currentPosition, steps) = Move(initialState, currentDirection, currentPosition);

                directions += steps;
                directions += ",";
            }

            return directions;
        }

        private static string GetTurn(Direction currentDirection, Direction nextDirection) => (currentDirection, nextDirection) switch
        {
            (Direction.Up, Direction.Right) => "R,",
            (Direction.Up, Direction.Left) => "L,",
            (Direction.Right, Direction.Up) => "L,",
            (Direction.Right, Direction.Down) => "R,",
            (Direction.Down, Direction.Right) => "L,",
            (Direction.Down, Direction.Left) => "R,",
            (Direction.Left, Direction.Up) => "R,",
            (Direction.Left, Direction.Down) => "L,",
            _ => throw new Exception("Invalid direction")
        };

        private static bool TryGetNextDirection(State initialState, Direction currentDirection, (int x, int y) position, out Direction nextDirection)
        {
            nextDirection = currentDirection;

            var validDirections = currentDirection switch
            {
                Direction.Up => new[] { Direction.Right, Direction.Left },
                Direction.Right => new[] { Direction.Up, Direction.Down },
                Direction.Down => new[] { Direction.Right, Direction.Left },
                Direction.Left => new[] { Direction.Up, Direction.Down, },
                _ => throw new Exception("Invalid direction")
            };

            foreach (var validDirection in validDirections)
            {
                nextDirection = validDirection switch
                {
                    Direction.Up when initialState.Path.Contains((position.x, position.y - 1)) => Direction.Up,
                    Direction.Right when initialState.Path.Contains((position.x + 1, position.y)) => Direction.Right,
                    Direction.Down when initialState.Path.Contains((position.x, position.y + 1)) => Direction.Down,
                    Direction.Left when initialState.Path.Contains((position.x - 1, position.y)) => Direction.Left,
                    _ => nextDirection
                };
            }

            return nextDirection != currentDirection;
        }

        private static ((int x, int y) currentPosition, int steps) Move(State initialState, Direction currentDirection, (int x, int y) currentPosition)
        {
            var xOffset = currentDirection switch
            {
                Direction.Right => 1,
                Direction.Left => -1,
                _ => 0
            };

            var yOffset = currentDirection switch
            {
                Direction.Up => -1,
                Direction.Down => 1,
                _ => 0
            };

            var steps = 1;
            for (; steps < int.MaxValue; steps++)
            {
                if (!initialState.Path.Contains((currentPosition.x + (steps * xOffset), currentPosition.y + (steps * yOffset))))
                    break;
            }

            steps--;

            return ((currentPosition.x + (steps * xOffset), currentPosition.y + (steps * yOffset)), steps);
        }

        private static string SplitDirectionsIntoFunctions(string directions)
        {
            var functions = new string[5];

            functions[1] = FindLargestFunction(directions, directions.IndexOfAny(new[] { 'R', 'L' }));
            directions = directions.Replace(functions[1], "A,");
            functions[1] = functions[1].TrimEnd(',') + "\n";

            functions[2] = FindLargestFunction(directions, directions.IndexOfAny(new[] { 'R', 'L' }));
            directions = directions.Replace(functions[2], "B,");
            functions[2] = functions[2].TrimEnd(',') + "\n";

            functions[3] = FindLargestFunction(directions, directions.IndexOfAny(new[] { 'R', 'L' }));
            directions = directions.Replace(functions[3], "C,");
            functions[3] = functions[3].TrimEnd(',') + "\n";

            functions[0] = directions.TrimEnd(',') + "\n";

            functions[4] = "n\n";

            return string.Concat(functions);
        }

        private static string FindLargestFunction(string directions, int start)
        {
            // This can be optimized but the problem space is simple
            for (var i = directions.Length - 1; i > start; i--)
            {
                var substr = directions[start..i];
                if (substr.Length < 20 &&
                    substr.Last() == ',' &&
                    substr.IndexOfAny(new[] { 'A', 'B', 'C' }) == -1 &&
                    directions.LastIndexOf(substr, StringComparison.Ordinal) > start)
                {
                    return substr;
                }
            }

            return "";
        }

        private class State
        {
            public HashSet<(int x, int y)> Path { get; } = new();
            public (int x, int y) RobotPosition { get; set; }
            public Direction RobotDirection { get; set; }
        }

        private enum Direction
        {
            Up, Right, Down, Left
        }
    }
}