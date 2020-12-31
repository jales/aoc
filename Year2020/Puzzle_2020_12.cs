using System;
using AoC.Infrastructure.Puzzles;

#pragma warning disable 8509

namespace AoC.Year2020
{
	public sealed class Puzzle_2020_12 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1221, 59435);

            AddTest(@"F10
N3
F7
R90
F11", 25, 286);
        }

        private (string, int)[] Instructions { get; }

        public Puzzle_2020_12(string input)
        {
            Instructions = input
                .ParseLines(l => (instruction: l[..1], amount: int.Parse(l[1..])));
        }

        protected override object Part1()
        {
            var (dx, dy) = (1, 0);
            var (x, y) = (0, 0);

            foreach (var (instruction, amount) in Instructions)
            {
                (dx, dy) = (instruction, amount) switch
                {
                    ("R", 90)  => ( dy, -dx),
                    ("L", 270) => ( dy, -dx),
                    ("L", 90)  => (-dy,  dx),
                    ("R", 270) => (-dy,  dx),
                    ("R", 180) => (-dx, -dy),
                    ("L", 180) => (-dx, -dy),
                    _          => ( dx,  dy)
                };

                (x, y) = instruction switch
                {
                    "N" => (x              , y + amount     ),
                    "E" => (x + amount     , y              ),
                    "S" => (x              , y - amount     ),
                    "W" => (x - amount     , y              ),
                    "F" => (x + amount * dx, y + amount * dy),
                    _   => (x              , y              )
                };
            }

            return Math.Abs(x)+ Math.Abs(y);
        }

        protected override object Part2()
        {
            var (x, y) = (0, 0);
            var (wx, wy) = (10, 1);

            foreach (var (instruction, amount) in Instructions)
            {
                (wx, wy) = (instruction, amount) switch
                {
                    ("R", 90)  => ( wy, -wx),
                    ("L", 270) => ( wy, -wx),
                    ("L", 90)  => (-wy,  wx),
                    ("R", 270) => (-wy,  wx),
                    ("R", 180) => (-wx, -wy),
                    ("L", 180) => (-wx, -wy),
                    _          => ( wx,  wy)
                };

                (wx, wy) = instruction switch
                {
                    "N" => (wx              , wy + amount     ),
                    "E" => (wx + amount     , wy              ),
                    "S" => (wx              , wy - amount     ),
                    "W" => (wx - amount     , wy              ),
                    _   => (wx              , wy              )
                };

                (x, y) = instruction switch
                {
                    "F" => (x + amount * wx, y + amount * wy),
                    _   => (x              , y              )
                };
            }

            return Math.Abs(x) + Math.Abs(y);
        }
    }
}