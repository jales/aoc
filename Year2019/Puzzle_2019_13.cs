using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_13 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(226, 10800);
        }

        private IntcodeProgram Program { get; }

        public Puzzle_2019_13(string input)
        {
            Program = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var count = 0;

            Program.OutputHandler = (value, index) =>
            {
                if ((index + 1) % 3 == 0 && value == 2)
                {
                    count++;
                }
            };

            Program.Run();

            return count;
        }

        protected override object Part2()
        {
            Program.Memory.WriteTo(0, 2);

            var score = 0L;
            var paddle = -1L;
            var ball = -1L;

            Program.InputProvider = _ => ball == paddle ? 0 : ball < paddle ? -1 : 1;

            Program.OutputHandler = (value, index) =>
            {
                if ((index + 1) % 3 != 0) return;

                var x = Program.Outputs[index - 2];
                var y = Program.Outputs[index - 1];

                switch (x, y, value)
                {
                    case (-1, 0, _): score = value; break;
                    case (_, _, 3): paddle = x; break;
                    case (_, _, 4): ball = x; break;
                }
            };

            Program.Run();

            return score;
        }
    }
}