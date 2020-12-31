using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_06 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(6775, 3356);

            AddTest(@"abc

a
b
c

ab
ac

a
a
a
a

b", 11, 6);
        }

        private string[][] GroupAnswers { get; }

        public Puzzle_2020_06(string input)
        {
            GroupAnswers = input
                .LineGroups();
        }

        protected override object Part1()
        {
            return GroupAnswers
                .Select(group => group
                    .SelectMany(answer => answer)
                    .Distinct()
                    .Count())
                .Sum();
        }

        protected override object Part2()
        {
            return GroupAnswers
                .Select(group => group
                    .SelectMany(answer => answer)
                    .GroupBy(answer => answer)
                    .Count(answers => answers.Count() == group.Length))
                .Sum();
        }
    }
}