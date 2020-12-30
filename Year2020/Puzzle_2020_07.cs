using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_07 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(155, 54803);

            AddTest(@"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.", 4, 32);
        }

        private Dictionary<string, Dictionary<string, int>> RulesByBagColor { get; }

        public Puzzle_2020_07(string input)
        {
            RulesByBagColor = input
                .ParseLines(Parse)
                .ToDictionary(l => l.bagColor, l=> l.containedBags);
        }

        protected override object Part1()
        {
            return RulesByBagColor.Count(bag => CanContainShinyGoldBag(bag.Value));
        }

        private bool CanContainShinyGoldBag(Dictionary<string, int> containedBags)
        {
            return containedBags.ContainsKey("shiny gold") || containedBags.Keys.Any(c => CanContainShinyGoldBag(RulesByBagColor[c]));
        }

        protected override object Part2()
        {
            return GetTotalContainedBags("shiny gold");
        }

        private int GetTotalContainedBags(string color)
        {
            var total = 0;

            foreach(var (containedBag, quantity) in RulesByBagColor[color])
            {
                total += quantity + quantity * GetTotalContainedBags(containedBag);
            }

            return total;
        }

        private (string bagColor, Dictionary<string, int> containedBags) Parse(string arg)
        {
            // language=regex
            var bagColor = arg.Matches(@"\w+ \w+").First();

            // language=regex
            var containedBagsMatches = arg.Matches(@"[0-9]+ \w+ \w+").ToList();

            var containedBags = new Dictionary<string, int>();

            foreach (var match in containedBagsMatches)
            {
                var parts = match.Split(' ');

                containedBags[$"{parts[1]} {parts[2]}"] = int.Parse(parts[0]);
            }

            return (bagColor, containedBags);
        }
    }
}