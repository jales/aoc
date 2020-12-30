using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_21 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(19350375, 1143990055);
        }

        private IntcodeProgram Program { get; }

        public Puzzle_2019_21(string input)
        {
            Program = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var instructions = new[]
            {
                // Jump if there is a hole on any of the next three tiles and the landing is safe
                // J = (!A || !B || !C) && D
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
                "WALK"
            };

            foreach (var c in string.Join("\n", instructions) + '\n')
            {
                Program.AddInput(c);
            }

            Program.Run();

            return Program.LastOutput;
        }

        protected override object Part2()
        {
            var instructions = new[]
            {
                // Jump if there is a hole on any of the next three tiles and the landing is safe
                // And can run to next or do another jum
                // J = (!A || !B || !C) && D && (E || H)
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
                "NOT E T",
                "NOT T T",
                "OR H T",
                "AND T J",
                "RUN"
            };

            foreach (var c in string.Join("\n", instructions) + '\n')
            {
                Program.AddInput(c);
            }

            Program.Run();

            return Program.LastOutput;

        }
    }
}