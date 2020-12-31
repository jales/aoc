using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_14 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(857266, 2144702);
        }

        private Dictionary<string, List<(string chemical, int quantity)>> RequirementsByChemical { get; }
        private Dictionary<string, int> RatiosByChemical { get; }

        public Puzzle_2019_14(string input)
        {
            var (requirementsByChemical, ratiosByChemical) = Parse(input);

            RequirementsByChemical = requirementsByChemical;

            RatiosByChemical = ratiosByChemical;
        }

        protected override object Part1()
        {
            var oreRequirement = GetOreRequirements(1);

            return oreRequirement;
        }

        protected override object Part2()
        {
            var low = 1L;
            var high = 1000000000000L;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var ore = GetOreRequirements(middle);

                if (ore < 1_000_000_000_000)
                {
                    low = middle + 1;
                }
                else if (ore > 1_000_000_000_000)
                {
                    high = middle - 1;
                }
                else
                {
                    return middle;
                }
            }

            return high;
        }

        private long GetOreRequirements(long fuelQuantity)
        {
            var balances = RatiosByChemical.ToDictionary(x => x.Key, _ => 0L);
            balances["FUEL"] = fuelQuantity;
            balances["ORE"] = 0;

            while (balances.TryFirst((k, v) => v > 0 && k != "ORE", out var kvp))
            {
                var (chemical, quantity) = kvp;

                var ratio = RatiosByChemical[chemical];

                var multiplier = (quantity + (ratio - 1)) / ratio;

                balances[chemical] = quantity - (ratio * multiplier);

                foreach (var (c, q) in RequirementsByChemical[chemical])
                {
                    balances[c] += (q * multiplier);
                }
            }

            return balances["ORE"];
        }

        private static (Dictionary<string, List<(string chemical, int quantity)>> requirementsByChemical, Dictionary<string, int> ratiosByChemical) Parse(string input)
        {
            var lines = input
               .Lines()
               .Select(l => l.Split(new[] { ' ', ',', '=', '>' }, StringSplitOptions.RemoveEmptyEntries)
                             .Buffer(2)
                             .Select(b => (chemical: b[1], quantity: int.Parse(b[0])))
                             .ToList())
               .ToList();

            var requirementsByChemical = lines
               .ToDictionary(
                    x => x.Last().chemical,
                    x => x.SkipLast(1).ToList());

            var ratiosByChemical = lines
               .ToDictionary(
                    x => x.Last().chemical,
                    x => x.Last().quantity);

            return (requirementsByChemical, ratiosByChemical);
        }
    }
}