using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_20 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(776160, 786240);
        }

        private int Target { get; }

        public Puzzle_2015_20(string input)
        {
            Target = input.ToInt32();
        }

        protected override object Part1()
        {
            for (var i = 1; i < int.MaxValue; i++)
            {
                var totalPresents = 0;

                foreach (var divisor in i.GetDivisors())
                {
                    totalPresents += divisor * 10;
                    if (totalPresents >= Target)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        protected override object Part2()
        {
            var divisors = new Dictionary<int, int>();

            for (var i = 1; i < int.MaxValue; i++)
            {
                var totalPresents = 0;

                foreach (var divisor in i.GetDivisors())
                {
                    divisors.TryAdd(divisor, 0);

                    divisors[divisor] += 1;

                    if (divisors[divisor] <= 50)
                    {
                        totalPresents += divisor * 11;
                        if (totalPresents >= Target)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }
    }
}