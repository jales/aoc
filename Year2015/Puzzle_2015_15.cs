using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_15 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(222870, 117936);
        }

        private (int capacity, int durability, int flavor, int texture, int calories)[] Ingredients { get; }

        public Puzzle_2015_15(string input)
        {
            Ingredients = input
               .ParseLines(line => line
                   .Int32Matches()
                   .Parse(matches => (matches[0], matches[1], matches[2], matches[3], matches[4])));
        }

        protected override object Part1()
        {
            var bestScore = 0;

            for (var i = 0; i < 101; i++)
            for (var j = 0; j < 101 - i; j++)
            for (var k = 0; k < 101 - (i+j); k++)
            {
                var l = 100 - (i + j + k);

                var totalScore = CalculateTotalScore(i, j, k, l);

                bestScore = Math.Max(bestScore, totalScore);
            }

            return bestScore;
        }

        protected override object Part2()
        {
            var bestScore = 0;

            for (var i = 0; i < 101; i++)
            for (var j = 0; j < 101 - i; j++)
            for (var k = 0; k < 101 - (i+j); k++)
            {
                var l = 100 - (i + j + k);

                var totalCalories =
                    (Ingredients[0].calories * i) +
                    (Ingredients[1].calories * j) +
                    (Ingredients[2].calories * k) +
                    (Ingredients[3].calories * l);

                if (totalCalories != 500) continue;

                var totalScore = CalculateTotalScore(i, j, k, l);

                bestScore = Math.Max(bestScore, totalScore);
            }

            return bestScore;
        }

        private int CalculateTotalScore(int i, int j, int k, int l)
        {
            var totalCapacity = Math.Max(0,
                (Ingredients[0].capacity * i) +
                (Ingredients[1].capacity * j) +
                (Ingredients[2].capacity * k) +
                (Ingredients[3].capacity * l));

            var totalDurability = Math.Max(0,
                (Ingredients[0].durability * i) +
                (Ingredients[1].durability * j) +
                (Ingredients[2].durability * k) +
                (Ingredients[3].durability * l));

            var totalFlavor = Math.Max(0,
                (Ingredients[0].flavor * i) +
                (Ingredients[1].flavor * j) +
                (Ingredients[2].flavor * k) +
                (Ingredients[3].flavor * l));

            var totalTexture = Math.Max(0,
                (Ingredients[0].texture * i) +
                (Ingredients[1].texture * j) +
                (Ingredients[2].texture * k) +
                (Ingredients[3].texture * l));

            return totalCapacity * totalDurability * totalFlavor * totalTexture;
        }
    }
}