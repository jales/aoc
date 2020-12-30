using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_02 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(7210630, 3892);
        }

        private IntcodeProgram Program { get; }

        public Puzzle_2019_02(string input)
        {
            Program = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            Program.Memory.WriteTo(1, 12);
            Program.Memory.WriteTo(2, 2);

            Program.Run();

            return Program.Memory.ReadFrom(0);
        }

        protected override object Part2()
        {
            for (var noun = 0; noun < 100; noun++)
            for (var verb = 0; verb < 100; verb++)
            {
                var p = new IntcodeProgram(Program);

                p.Memory.WriteTo(1, noun);
                p.Memory.WriteTo(2, verb);

                p.Run();

                if(p.Memory.ReadFrom(0) == 19690720)
                {
                    return noun * 100 + verb;
                }
            }

            return -1;
        }
    }
}