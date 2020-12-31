using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AoC.Infrastructure.Puzzles;
using static System.Numerics.BigInteger;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_22 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(new BigInteger(3377), Parse("29988879027217"));
        }

        private string Input { get; }

        public Puzzle_2019_22(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            BigInteger deckSize = 10007;
            BigInteger iterations = 1;

            var (a, b) = GetShuffle(deckSize, iterations);

            // y = ax + b
            var position = Mod(a * 2019 + b, deckSize);

            return position;
        }

        protected override object Part2()
        {
            BigInteger deckSize = 119315717514047;
            BigInteger iterations = 101741582076661;

            var (a, b) = GetShuffle(deckSize, iterations);

            // y = ax+b <=> (y-b)/a = x
            var card = Mod((2020 - b) * ModInv(a, deckSize), deckSize);

            return card;
        }

        private (BigInteger a, BigInteger b) GetShuffle(in BigInteger deckSize, in BigInteger iterations)
        {
            var a = One;
            var b = Zero;

            // c(ax+b)+d = cax+cb+d
            foreach (var (c, d) in GetShuffleTechniques(deckSize))
            {
                a = Mod(c * a, deckSize);
                b = Mod(c * b + d, deckSize);
            }

            // a(a(a(a(a(a(...)+b)+b)+b)+b)+b)+b = (a^k)x + b((1-a^k)/(1-a))
            // ak = a^k
            // bk = b((1-ak)/(1-a))
            var ak = ModPow(a, iterations, deckSize);
            var bk = Mod(b * (1 - ak) * ModInv(1 - a, deckSize), deckSize);

            return (ak, bk);
        }

        private IEnumerable<(BigInteger, BigInteger)> GetShuffleTechniques(BigInteger deckSize) => Input
               .Lines()
               .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
               .Select(parts => (parts[0], parts[1]) switch
               {
                   ("deal", "into") => (MinusOne, deckSize - 1),         // -ax + M - 1
                   ("cut", _) => (One, -Parse(parts.Last())), // x - n
                   ("deal", "with") => (Parse(parts.Last()), Zero),    // nx
                   _ => throw new Exception("Invalid shuffle technique")
               });
    }
}