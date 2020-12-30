using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;
using static System.Math;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_07 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(914828, 17956613);
        }

        private IntcodeProgram OriginalProgram { get; }

        public Puzzle_2019_07(string input)
        {
            OriginalProgram = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var permutations = new List<int> { 0, 1, 2, 3, 4 }
               .Permutations()
               .Select(p => p.Select(x =>
               {
                   var program = new IntcodeProgram(OriginalProgram, x);

                   program.OutputHandler = (value, _) => program.AddInput(value);

                   return program;
               })
               .ToList());

            var maxOutput = long.MinValue;
            foreach (var machines in permutations)
            {
                var lastOutput = 0L;
                foreach (var machine in machines)
                {
                    machine.AddInput(lastOutput);
                    machine.Run();
                    lastOutput = machine.LastOutput;
                }
                maxOutput = Max(maxOutput, lastOutput);
            }

            return maxOutput;
        }

        protected override object Part2()
        {
            var maxOutput = long.MinValue;

            var permutations = new List<int> { 5, 6, 7, 8, 9 }
               .Permutations()
               .Select(p => p.Select(x => new IntcodeProgram(OriginalProgram, x)).ToList());

            foreach (var combination in permutations)
            {
                var lastOutput = 0L;
                while (true)
                {
                    foreach (var computer in combination)
                    {
                        computer.AddInput(lastOutput);
                        computer.Step(InterruptMode.Output);
                        lastOutput = computer.LastOutput;
                    }

                    if (combination.Last().IsTerminated)
                        break;
                }

                maxOutput = Max(maxOutput, lastOutput);
            }

            return maxOutput;
        }
    }
}