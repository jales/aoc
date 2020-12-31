using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_19 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(138, 13530764);
        }

        private IntcodeProgram OriginalProgram { get; }

        public Puzzle_2019_19(string input)
        {
            OriginalProgram = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var points = 0L;

            for (var y = 0; y < 50; y++)
            for (var x = 0; x < 50; x++)
            {
                if (IsAttracted(x, y)) points++;
            }

            return points;
        }

        protected override object Part2()
        {
            var x = 0;
            var y = 99;

            while (true)
            {
                if (IsAttracted(x, y))
                {
                    if (IsAttracted(x + 99, y - 99))
                    {
                        return x * 10000 + (y - 99);
                    }

                    y++;
                }
                else
                {
                    x++;
                }
            }
        }

        private bool IsAttracted(int x, int y)
        {
            var p = new IntcodeProgram(OriginalProgram, x, y);
            p.Run();
            return p.LastOutput == 1;
        }
    }
}