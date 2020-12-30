using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_08 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1563, 767);

            AddTest(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6", 5, 8);
        }

        private Program OriginalProgram { get; }

        public Puzzle_2020_08(string input)
        {
            OriginalProgram = new Program(input
                .ParseLines(Instruction.Parse)
                .ToList());
        }

        protected override object Part1()
        {
            return OriginalProgram.Run().result;
        }

        protected override object Part2()
        {
            for (var i = 0; i < OriginalProgram.Instructions.Count; i++)
            {
                var p = OriginalProgram with { Instructions = ModifyInstructions(OriginalProgram.Instructions, i) };

                var (terminated, result) = p.Run();

                if (terminated) return result;
            }
            return -1;

            static List<Instruction> ModifyInstructions(List<Instruction> originalInstructions, int i)
            {
                var instructions = originalInstructions.ToList();

                instructions[i] = instructions[i] switch
                {
                    ("nop", _) => instructions[i] with { Code = "jmp" },
                    ("jmp", _) => instructions[i] with { Code = "nop" },
                    _          => instructions[i]
                };


                return instructions;
            }
        }

        public record Instruction(string Code, int Arg0)
        {
            public static Instruction Parse(string description)
            {
                var parts = description.Split(' ');

                return new(parts[0], int.Parse(parts[1]));
            }
        }

        private record Program(List<Instruction> Instructions)
        {
            public (bool terminated, int result) Run()
            {
                var instructionPointer = 0;
                var result = 0;

                var visitedInstructions = new HashSet<int>();
                while (true)
                {
                    if (instructionPointer >= Instructions.Count) return (true, result);

                    if (!visitedInstructions.Add(instructionPointer)) return (false, result);

                    result += Instructions[instructionPointer] switch
                    {
                        ("acc", var increment) => increment,
                        _ => 0
                    };

                    instructionPointer += Instructions[instructionPointer] switch
                    {
                        ("jmp", var offset) => offset,
                        _ => 1
                    };
                }
            }
        }
    }
}