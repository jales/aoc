using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_23 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(43896725, 2911418906);

            AddTest("389125467", 67384529, 149245887792);
        }

        private int[] CupsSuccessor { get; }
        private int CurrentCup { get; }
        private int LastCup { get; }
        private int MaxCup { get; }

        public Puzzle_2020_23(string input)
        {
            var numbers = input.ToArray().ParseArray(c => c - '0');

            CurrentCup = numbers[0];
            MaxCup = numbers.Max();
            LastCup = numbers[^1];

            CupsSuccessor = new int[numbers.Length+1];

            CupsSuccessor[0] = int.MaxValue; // Serves as a guard if we ever mess with 0; It was useful during debugging :P

            for (var i = 0; i < numbers.Length; i++)
            {
                CupsSuccessor[numbers[i]] = numbers[(i + 1) % numbers.Length];
            }
        }

        protected override object Part1()
        {
            var currentCup = CurrentCup;

            for (var move = 0; move < 100; move++)
            {
                var c1 = CupsSuccessor[currentCup];
                var c2 = CupsSuccessor[c1];
                var c3 = CupsSuccessor[c2];

                var destinationCup = currentCup != 1 ? currentCup -1 : MaxCup;
                while (destinationCup == c1 || destinationCup == c2 || destinationCup == c3)
                {
                    destinationCup = destinationCup != 1 ? destinationCup -1 : MaxCup;
                }

                var c4 = CupsSuccessor[destinationCup];
                var c5 = CupsSuccessor[c3];

                CupsSuccessor[destinationCup] = c1; // Destination cup now points to the first of the three cups.
                CupsSuccessor[c3] = c4;             // The last of the three cups now point to the cup that the destination cup was pointing to
                CupsSuccessor[currentCup] = c5;     // The current cup now point to the cup the last of the three cups was pointing to

                currentCup = CupsSuccessor[currentCup];
            }

            var cup = 1;
            var result = 0;

            while (CupsSuccessor[cup] != 1)
            {
                cup = CupsSuccessor[cup];
                result = result * 10 + cup;
            }

            return result;
        }

        protected override object Part2()
        {
            var currentCup = CurrentCup;

            var cupsSuccessor = ExpandCupsForPart2();

            for (var move = 0; move < 10_000_000; move++)
            {
                var c1 = cupsSuccessor[currentCup];
                var c2 = cupsSuccessor[c1];
                var c3 = cupsSuccessor[c2];

                var destinationCup = currentCup != 1 ? currentCup - 1 : 1_000_000;
                while (destinationCup == c1 || destinationCup == c2 || destinationCup == c3)
                {
                    destinationCup = destinationCup != 1 ? destinationCup - 1 : 1_000_000;
                }

                var c4 = cupsSuccessor[destinationCup];
                var c5 = cupsSuccessor[c3];

                cupsSuccessor[destinationCup] = c1; // Destination cup now points to the first of the three cups.
                cupsSuccessor[c3] = c4;             // The last of the three cups now point to the cup that the destination cup was pointing to
                cupsSuccessor[currentCup] = c5;     // The current cup now point to the cup the last of the three cups was pointing to

                currentCup = cupsSuccessor[currentCup];
            }


            return (long)cupsSuccessor[1] * cupsSuccessor[cupsSuccessor[1]];
        }

        private int[] ExpandCupsForPart2()
        {
            var cupsSuccessor = new int[1_000_001];
            Array.Copy(CupsSuccessor, cupsSuccessor, CupsSuccessor.Length);

            for (var i = CupsSuccessor.Length; i < cupsSuccessor.Length - 1; i++)
            {
                cupsSuccessor[i] = i + 1;
            }

            cupsSuccessor[^1] = CupsSuccessor[LastCup];    // Point the new last cup to what the old one was pointing at
            cupsSuccessor[LastCup] = CupsSuccessor.Length; // Point the old last cup to the first new cup

            return cupsSuccessor;
        }
    }
}