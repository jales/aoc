using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_23 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(184, 231);
        }

        private (string instructions, string arg1, string? arg2)[] Instructions { get; }
        private Dictionary<string, int> Registers { get; }

        public Puzzle_2015_23(string input)
        {
            Instructions = input
               .ParseLines(line => line
                   .Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries)
                   .Parse(p => (p[0], p[1], p.Length == 3 ? p[2] : null)));

            Registers = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0
            };
        }

        protected override object Part1()
        {
            RunProgram();

            return Registers["b"];
        }

        protected override object Part2()
        {
            Registers["a"] = 1;

            RunProgram();

            return Registers["b"];
        }

        private void RunProgram()
        {
            for (var i = 0; i < Instructions.Length; i++)
            {
                switch (Instructions[i])
                {
                    case ("hlf", var register, _):
                        Registers[register] /= 2;
                        break;
                    case ("tpl", var register, _):
                        Registers[register] *= 3;
                        break;
                    case ("inc", var register, _):
                        Registers[register] += 1;
                        break;
                    case ("jmp", var offset, _):
                        i += (int.Parse(offset) - 1); // -1 to account for loop increment
                        break;
                    case ("jie", var register, var offset):
                        if (Registers[register] % 2 == 0)
                            i += (int.Parse(offset!) - 1); // -1 to account for loop increment
                        break;
                    case ("jio", var register, var offset):
                        if (Registers[register] == 1)
                            i += (int.Parse(offset!) - 1); // -1 to account for loop increment
                        break;
                }
            }
        }
    }
}