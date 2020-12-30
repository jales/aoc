using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_22 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(32413, 31596);

            AddTest(@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10",
                306,
                291);
        }

        private Queue<int> Player1Deck { get; }
        private Queue<int> Player2Deck { get; }

        public Puzzle_2020_22(string input)
        {
            var groups = input.LineGroups();

            Player1Deck = new Queue<int>(groups[0][1..].ParseArray(int.Parse));
            Player2Deck = new Queue<int>(groups[1][1..].ParseArray(int.Parse));
        }

        protected override object Part1()
        {
            var player1Won = Play(Player1Deck, Player2Deck, false);

            return Score(player1Won ? Player1Deck : Player2Deck);
        }

        protected override object Part2()
        {
            var player1Won = Play(Player1Deck, Player2Deck, true);

            return Score(player1Won ? Player1Deck : Player2Deck);
        }

        private static bool Play(Queue<int> p1Deck, Queue<int> p2Deck, bool recurse)
        {
            var p1PreviousDecks = new HashSet<string>();
            var p2PreviousDecks = new HashSet<string>();

            while (p1Deck.Count > 0 && p2Deck.Count > 0)
            {
                if (recurse && (!p1PreviousDecks.Add(string.Join(",", p1Deck)) || !p2PreviousDecks.Add(string.Join(",", p2Deck))))
                {
                    return true;
                }

                var p1Card = p1Deck.Dequeue();

                var p2Card = p2Deck.Dequeue();

                var p1Won = (recurse && p1Deck.Count >= p1Card && p2Deck.Count >= p2Card)
                    ? Play(new Queue<int>(p1Deck.Take(p1Card)), new Queue<int>(p2Deck.Take(p2Card)), recurse)
                    : p1Card > p2Card;

                if (p1Won)
                {
                    p1Deck.Enqueue(p1Card);
                    p1Deck.Enqueue(p2Card);
                }
                else
                {
                    p2Deck.Enqueue(p2Card);
                    p2Deck.Enqueue(p1Card);
                }
            }

            return p1Deck.Count > 0;
        }

        private static long Score(Queue<int> deck)
        {
            var index = deck.Count;
            return deck.Sum(card => card * index--);
        }
    }
}