using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_24 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(10723906903, 74850409);
        }

        private long[] Packages { get; }

        public Puzzle_2015_24(string input)
        {
            Packages = input.ParseLines(long.Parse);
        }

        protected override object Part1()
        {
            var targetWeight = Packages.Sum() / 3;

            var smallestQuantumEntanglement = long.MaxValue;

            for (var i = 6; i < Packages.Length-12; i++)
            {
                foreach (var group1 in Packages.Combinations(i).Where(c => c.Sum() == targetWeight))
                {
                    var quantumEntanglement = group1.Aggregate((x, y) => x * y);

                    if (smallestQuantumEntanglement <= quantumEntanglement) continue;

                    if(HasValidRemainingGroups(targetWeight, Packages.Except(group1).ToList(), 2, 6))
                    {
                        smallestQuantumEntanglement = quantumEntanglement;
                    }

                }

                if (smallestQuantumEntanglement != long.MaxValue) return smallestQuantumEntanglement;
            }

            return -1;
        }

        protected override object Part2()
        {
            var targetWeight = Packages.Sum() / 4;

            var smallestQuantumEntanglement = long.MaxValue;

            for (var i = 4; i < Packages.Length-12; i++)
            {
                foreach (var group1 in Packages.Combinations(i).Where(c => c.Sum() == targetWeight))
                {
                    var quantumEntanglement = group1.Aggregate((x, y) => x * y);

                    if (smallestQuantumEntanglement <= quantumEntanglement) continue;

                    if(HasValidRemainingGroups(targetWeight, Packages.Except(group1).ToList(), 3, 6))
                    {
                        smallestQuantumEntanglement = quantumEntanglement;
                    }

                }

                if (smallestQuantumEntanglement != long.MaxValue) return smallestQuantumEntanglement;
            }

            return -1;
        }

        private static bool HasValidRemainingGroups(long targetWeight, IReadOnlyCollection<long> packages, int remainingGroups, int minK)
        {
            if(remainingGroups == 2)
            {
                var maxK = packages.Count - minK;
                if (packages.Combinations(minK, maxK).Any(c => c.Sum() == targetWeight))
                {
                    return true;
                }
            }
            else
            {
                var maxK = packages.Count - minK;
                foreach (var group in packages.Combinations(minK, maxK).Where(c => c.Sum() == targetWeight))
                {
                    if (HasValidRemainingGroups(targetWeight, packages.Except(group).ToList(), remainingGroups - 1, minK))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}