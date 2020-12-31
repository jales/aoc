using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support;

#pragma warning disable 8524

namespace AoC.Year2020
{
	public sealed class Puzzle_2020_11 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(2296, 2089);

            AddTest(@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL", 37, 26);
        }

        private SparseGrid<SeatState> CurrentState { get; set; }

        public Puzzle_2020_11(string input)
        {
            CurrentState = input.ToSparseGrid(c => c switch
            {
                'L' => (true, SeatState.Empty),
                _   => (false, default)
            });
        }

        private enum SeatState { Empty, Occupied }

        protected override object Part1()
        {
            while (true)
            {
                var nextState = CurrentState.Evolve((x, y, current) => (current, CurrentState.Neighbors(x, y)) switch
                {
                    (SeatState.Empty, var neighbors)    => neighbors.None(s => s.value == SeatState.Occupied)
                                                               ? SeatState.Occupied
                                                               : SeatState.Empty,

                    (SeatState.Occupied, var neighbors) => neighbors.Count(s => s.value == SeatState.Occupied) >= 4
                                                               ? SeatState.Empty
                                                               : SeatState.Occupied
                });

                if(nextState == CurrentState) break;
                CurrentState = nextState;
            }

            return CurrentState.Count(s => s == SeatState.Occupied);
        }

        protected override object Part2()
        {
            while (true)
            {
                var nextState = CurrentState.Evolve((x, y, current) => (current, CurrentState.SlopesFrom(x, y)) switch
                {
                    (SeatState.Empty, var slopes)    => slopes.Where(s => s.Any())
                                                              .Select(s => s.First())
                                                              .None(s => s.value == SeatState.Occupied)
                                                            ? SeatState.Occupied
                                                            : SeatState.Empty,

                    (SeatState.Occupied, var slopes) => slopes.Where(s => s.Any())
                                                              .Select(s => s.First())
                                                              .Count(s => s.value == SeatState.Occupied) >= 5
                                                            ? SeatState.Empty
                                                            : SeatState.Occupied
                });

                if(nextState == CurrentState) break;
                CurrentState = nextState;
            }

            return CurrentState.Count(s => s == SeatState.Occupied);
        }
    }
}