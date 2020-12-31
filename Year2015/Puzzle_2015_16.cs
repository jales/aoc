using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_16 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(373, 260);
        }

        private Dictionary<string, Dictionary<int, List<int>>> Facts { get; }
        private Dictionary<string, int> Clues { get; }

        public Puzzle_2015_16(string input)
        {
            Facts = new Dictionary<string, Dictionary<int, List<int>>>();

            foreach (var line in input.Lines())
            {
                var parts = line.Split(new[] {' ', ':', ','}, StringSplitOptions.RemoveEmptyEntries);

                var aunt = int.Parse(parts[1]);

                foreach (var index in new[] {2, 4, 6})
                {
                    var fact = parts[index];
                    var count = int.Parse(parts[index + 1]);

                    var auntsByFact = Facts.GetOrAdd(fact, () => new Dictionary<int, List<int>>());

                    var aunts = auntsByFact.GetOrAdd(count, () => new List<int>());

                    aunts.Add(aunt);
                }
            }

            Clues = new Dictionary<string, int>
            {
                ["children"] = 3,
                ["cats"] = 7,
                ["samoyeds"] = 2,
                ["pomeranians"] = 3,
                ["akitas"] = 0,
                ["vizslas"] = 0,
                ["goldfish"] = 5,
                ["trees"] = 3,
                ["cars"] = 2,
                ["perfumes"] = 1,
            };
        }

        protected override object Part1()
        {
            var aunts = new HashSet<int>(Enumerable.Range(1, 500));

            foreach (var (fact, targetValue) in Clues)
            foreach (var (value, auntsByFact) in Facts[fact])
            {
                if (targetValue != value)
                {
                    aunts.ExceptWith(auntsByFact);
                }
            }

            return aunts.Single();
        }

        protected override object Part2()
        {
            var aunts = new HashSet<int>(Enumerable.Range(1, 500));

            foreach (var (fact, targetValue) in Clues)
            foreach (var (value, auntsByFact) in Facts[fact])
            {
                switch (fact)
                {
                    case "cats":
                    case "trees":
                        if (targetValue >= value)
                        {
                            aunts.ExceptWith(auntsByFact);
                        }
                        break;
                    case "pomeranians":
                    case "goldfish":
                        if (targetValue <= value)
                        {
                            aunts.ExceptWith(auntsByFact);
                        }
                        break;
                    default:
                        if (targetValue != value)
                        {
                            aunts.ExceptWith(auntsByFact);
                        }
                        break;
                }
            }

            return aunts.Single();
        }
    }
}