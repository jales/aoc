using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_08 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1342, 2074);
        }

        private string[] Lines { get; }

        public Puzzle_2015_08(string input)
        {
            Lines = input.Lines();
        }

        protected override object Part1()
        {
            var codeLength = 0;
            var memoryLength = 0;

            foreach (var line in Lines)
            {
                codeLength += line.Length;

                for (var i = 1; i < line.Length-1; i++)
                {
                    if (line[i] == '\\')
                    {
                        i++;
                        if (line[i] == 'x')
                            i += 2;
                    }
                    memoryLength++;
                }
            }

            return codeLength - memoryLength;
        }

        protected override object Part2()
        {
            var codeLength = 0;
            var encodedLength = 0;
            foreach (var line in Lines)
            {
                codeLength += line.Length;
                encodedLength += 2;

                foreach (var c in line)
                {
                    if (c == '\\' || c == '"')
                    {
                        encodedLength++;
                    }
                    encodedLength++;
                }
            }

            return encodedLength - codeLength;
        }
    }
}
