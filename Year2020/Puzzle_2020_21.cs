using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_21 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution(2428, "bjq,jznhvh,klplr,dtvhzt,sbzd,tlgjzx,ctmbr,kqms");

            AddTest(@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)", 5, "mxmxvkd,sqjhc,fvjkl");
        }

        private Dictionary<string, int> IngredientCount { get; } = new();
        private Dictionary<string, int> AllergenCount { get; } = new();
        private Dictionary<string, HashSet<string>> IngredientAllergens { get; } = new();
        private Dictionary<string, List<string[]>> AllergenIngredientLists { get; } = new();

        public Puzzle_2020_21(string input)
        {
            foreach (var line in input.Lines())
            {
                var words = line.Matches(@"\w+");
                var separator = Array.IndexOf(words, "contains");

                foreach (var ingredient in words[..separator])
                {
                    IngredientCount[ingredient] = IngredientCount.GetOrAdd(ingredient, 0) + 1;

                    foreach (var allergen in words[(separator + 1)..])
                    {
                        IngredientAllergens.GetOrAdd(ingredient, _ => new ()).Add(allergen);
                    }
                }

                foreach (var allergen in words[(separator+1)..])
                {
                    AllergenCount[allergen] = AllergenCount.GetOrAdd(allergen, 0) + 1;

                    AllergenIngredientLists.GetOrAdd(allergen, _=> new ()).Add(words[..separator]);
                }
            }
        }

        protected override object Part1()
        {
            var count = 0;
            foreach (var (ingredient, allergens) in IngredientAllergens)
            {
                var ingredientCount = IngredientCount[ingredient];

                allergens.RemoveWhere(allergen => AllergenCount[allergen] > ingredientCount);
                allergens.RemoveWhere(allergen => AllergenIngredientLists[allergen].Any(list => !list.Contains(ingredient)));

                if (allergens.Count == 0) count += ingredientCount;
            }

            return count;
        }

        protected override object Part2()
        {
            var allergenIngredient = new Dictionary<string, string>();

            while (true)
            {
                var knownIngredientAllergen = IngredientAllergens.Where(kvp => kvp.Value.Count == 1).ToList();

                if(knownIngredientAllergen.Count == 0) break;

                foreach (var (ingredient, allergens) in knownIngredientAllergen)
                {
                    var knownAllergen = allergens.Single();

                    allergenIngredient[knownAllergen] = ingredient;

                    IngredientAllergens.Remove(ingredient);

                    foreach (var (_, otherAllergens) in IngredientAllergens)
                    {
                        otherAllergens.Remove(knownAllergen);
                    }
                }
            }

            return string.Join(",", allergenIngredient.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
        }
    }
}
