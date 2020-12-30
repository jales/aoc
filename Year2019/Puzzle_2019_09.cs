using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_09 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(4288078517L, 69256);
        }

        private IntcodeProgram Program { get; }

        public Puzzle_2019_09(string input)
        {
            Program = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            Program.AddInput(1);

            Program.Run();

            return Program.LastOutput;
        }

        protected override object Part2()
        {
            Program.AddInput(2);

            Program.Run();

            return Program.LastOutput;
        }
    }
}