using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_10 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(252594, 3579328);
        }

        private List<int> Sequence { get; }

        public Puzzle_2015_10(string input)
        {
            Sequence = input.Select(c => int.Parse("" + c)).ToList();
        }

        protected override object Part1()
        {
            var lookAndSay = Sequence;

            for (var i = 0; i < 40; i++)
            {
                lookAndSay = LookAndSay(lookAndSay);
            }

            return lookAndSay.Count;
        }

        protected override object Part2()
        {
            var lookAndSay = Sequence;

            for (var i = 0; i < 50; i++)
            {
                lookAndSay = LookAndSay(lookAndSay);
            }

            return lookAndSay.Count;
        }

        private static List<int> LookAndSay(List<int> current)
        {
            var digit = current[0];
            var repeatedCount = 1;
            var next = new List<int>(current.Count);

            for (var j = 1; j < current.Count; j++)
            {
                if (digit == current[j])
                {
                    repeatedCount++;
                }
                else
                {
                    next.Add(repeatedCount);
                    next.Add(digit);
                    digit = current[j];
                    repeatedCount = 1;
                }
            }

            next.Add(repeatedCount);
            next.Add(digit);
            return next;
        }
    }
}