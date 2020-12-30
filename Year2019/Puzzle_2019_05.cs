using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_05 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(15386262, 10376124);
        }

        private IntcodeProgram Program { get; }

        public Puzzle_2019_05(string input)
        {
            Program = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            Program.AddInput(1);
            Program.Run();
            return Program.Outputs.Last();
        }

        protected override object Part2()
        {
            Program.AddInput(5);
            Program.Run();
            return Program.Outputs.Last();
        }
    }
}