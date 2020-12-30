using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    internal sealed class Puzzle_2016_06 : Puzzle
    {
        public static void Configure()
        {
            SetSolution("bjosfbce", "veqfxzfx");

            AddTest(@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar", "easter", "advent");
        }

        private char[][] Messages { get; }
        private int MessageLength { get; }

        public Puzzle_2016_06(string input)
        {
            Messages = input
                .ParseLines(l => l.ToArray());

            MessageLength = Messages[0].Length;
        }

        protected override object Part1()
        {
            var message = new char[MessageLength];

            for(var i = 0; i < MessageLength; i++)
            {
                message[i] = Messages
                    .Select(m => m[i])
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .First();
            }

            return new string(message);
        }

        protected override object Part2()
        {
            var message = new char[MessageLength];

            for (var i = 0; i < MessageLength; i++)
            {
                message[i] = Messages
                    .Select(m => m[i])
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Last();
            }

            return new string(message);
        }
    }
}