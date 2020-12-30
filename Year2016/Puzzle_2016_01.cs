using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    internal sealed class Puzzle_2016_01 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(298, 158);
        }

        private (char turn, int blocks)[] Directions { get; }

        public Puzzle_2016_01(string input)
        {
            Directions = input.ParseMatches("[LR][0-9]+", match => (match[0], match[1..].ToInt32()));
        }

        protected override object Part1()
        {
            var (direction, x, y) = (0, 0, 0);

            foreach (var (turn, blocks) in Directions)
            {
                (direction, x, y) = (direction, turn) switch
                {
                    (0, 'R') => (1, x + blocks, y         ),
                    (0, 'L') => (3, x - blocks, y         ),
                    (1, 'R') => (2, x         , y - blocks),
                    (1, 'L') => (0, x         , y + blocks),
                    (2, 'R') => (3, x - blocks, y         ),
                    (2, 'L') => (1, x + blocks, y         ),
                    (3, 'R') => (0, x         , y + blocks),
                    (3, 'L') => (2, x         , y - blocks),
                    _        => (direction, x, y)
                };
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        protected override object Part2()
        {
            var (direction, x, y) = (0, 0, 0);
            var visitedPositions = new HashSet<(int, int)> {(0, 0)};

            foreach (var (turn, blocks) in Directions)
            {
                var (newDirection, dx, dy) = (direction, turn) switch
                {
                    (0, 'R') => (1,  1,  0),
                    (0, 'L') => (3, -1,  0),
                    (1, 'R') => (2,  0, -1),
                    (1, 'L') => (0,  0,  1),
                    (2, 'R') => (3, -1,  0),
                    (2, 'L') => (1,  1,  0),
                    (3, 'R') => (0,  0,  1),
                    (3, 'L') => (2,  0, -1),
                    _        => (direction, 0, 0)
                };

                direction = newDirection;



                for (var i = 0; i < blocks; i++)
                {
                    x += dx;
                    y += dy;

                    if (!visitedPositions.Add((x, y)))
                    {
                        return Math.Abs(x) + Math.Abs(y);
                    }
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }
    }
}
