using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_25 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(711945, MerryChristmas);

            AddPart1Test(@"5764801
17807724", 14897079);
        }

        private long CardPublicKey { get; }
        private long DoorPublicKey { get; }

        public Puzzle_2020_25(string input)
        {
            var numbers = input.ParseLines(int.Parse);

            CardPublicKey = numbers[0];
            DoorPublicKey = numbers[1];
        }

        protected override object Part1()
        {
            var cardLoopSize = 0L;
            for (var key = 1; key != CardPublicKey; key = (key * 7) % 20201227, cardLoopSize++) { }

            var handshake = 1L;
            for (var i = 0; i < cardLoopSize; handshake = (handshake * DoorPublicKey) % 20201227, i++) { }

            return handshake;
        }

        protected override object Part2()
        {
            return MerryChristmas;
        }
    }
}