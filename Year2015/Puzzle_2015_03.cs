using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_03 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(2565, 2639);
        }

        private string Input { get; }

        public Puzzle_2015_03(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            (int x, int y) currentPosition = (0, 0);
            var visitedHouses = new HashSet<(int, int)> { currentPosition };

            foreach (var instruction in Input)
            {
                currentPosition = instruction switch
                {
                    '^' => (currentPosition.x    , currentPosition.y + 1),
                    '>' => (currentPosition.x + 1, currentPosition.y    ),
                    'v' => (currentPosition.x    , currentPosition.y - 1),
                    '<' => (currentPosition.x - 1, currentPosition.y    ),
                    _   => throw new InvalidOperationException(),
                };
                visitedHouses.Add(currentPosition);
            }

            return visitedHouses.Count;
        }

        protected override object Part2()
        {
            (int x, int y) currentSantaPosition = (0, 0);
            (int x, int y) currentRobotPosition = (0, 0);
            var visitedHouses = new HashSet<(int, int)> { currentSantaPosition };

            for (var index = 0; index < Input.Length; index++)
            {
                var instruction = Input[index];
                var position = index % 2 == 0 ? currentSantaPosition : currentRobotPosition;

                position = instruction switch
                {
                    '^' => (position.x, position.y + 1),
                    '>' => (position.x + 1, position.y),
                    'v' => (position.x, position.y - 1),
                    '<' => (position.x - 1, position.y),
                    _   => throw new InvalidOperationException(),
                };
                visitedHouses.Add(position);

                if (index % 2 == 0)
                {
                    currentSantaPosition = position;
                }
                else
                {
                    currentRobotPosition = position;
                }
            }

            return visitedHouses.Count;
        }
    }
}
