using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_13 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(664, 640);
        }

        private HashSet<string> Guests { get; }
        private Dictionary<(string, string), int> HappinessByGuestPair { get; }

        public Puzzle_2015_13(string input)
        {
            Guests = new HashSet<string>();
            HappinessByGuestPair = new Dictionary<(string, string), int>();

            foreach (var line in input.Lines())
            {
                var parts = line.Split(' ', '.');

                Guests.Add(parts[0]);
                HappinessByGuestPair.Add((parts[0], parts[10]),  (parts[2] == "gain" ? 1 : -1) * parts[3].ToInt32());
            }
        }

        protected override object Part1()
        {
            return Guests.Permutations().Select(GetTotalHappiness).Max();
        }

        protected override object Part2()
        {
            foreach (var guest in Guests)
            {
                HappinessByGuestPair.Add((guest, "Me"), 0);
                HappinessByGuestPair.Add(("Me", guest), 0);
            }
            Guests.Add("Me");

            return Guests.Permutations().Select(GetTotalHappiness).Max();
        }

        private int GetTotalHappiness(IReadOnlyList<string> permutation)
        {
            var totalHappiness = 0;

            for (var i = 0; i < permutation.Count; i++)
            {
                var guest = permutation[i];
                var guestToTheLeft = (i < permutation.Count - 1) ? permutation[i + 1] : permutation[0];
                var guestToTheRight = (i == 0) ? permutation[^1] : permutation[i-1];

                var happiness = HappinessByGuestPair[(guest, guestToTheLeft)];
                var totalHappiness1 = HappinessByGuestPair[(guest, guestToTheRight)];

                totalHappiness += happiness;
                totalHappiness += totalHappiness1;
            }

            return totalHappiness;
        }
    }
}
