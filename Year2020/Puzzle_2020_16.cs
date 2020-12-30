using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_16 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution(26980, 3021381607403);

            AddPart1Test(@"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12", 71);
        }

        private Dictionary<string, Range[]> Ranges { get; }
        private int[] Ticket { get; }
        private int[][] NearbyTickets { get; }
        private HashSet<int[]> ValidTickets { get; } = new();

        public Puzzle_2020_16(string input)
        {
            var groups = input.LineGroups();

            Ranges = groups[0].ToDictionary(
                l => l.Split(':')[0],
                l => l.ParseMatches(@"\d+-\d+", match => match.Split('-').Parse(Range.Parse)));

            Ticket = groups[1][1].Int32Matches();

            NearbyTickets = groups[2][1..].ParseArray(l => l.Int32Matches());
        }

        protected override object Part1()
        {
            var allRanges = Ranges.Values.SelectMany(x => x).ToArray();

            var errorRate = 0;
            foreach (var ticket in NearbyTickets)
            {
                var ticketErrors = ticket.Where(number => allRanges.None(range => range.Contains(number))).Sum();
                if (ticketErrors > 0)
                {
                    errorRate += ticketErrors;
                }
                else
                {
                    ValidTickets.Add(ticket);
                }
            }

            return errorRate;
        }

        protected override object Part2()
        {
            var potentialFieldsByTicketPosition = Ticket.Select(_ => Ranges.Keys.ToList()).ToArray();

            foreach (var validTicket in ValidTickets)
            {
                for (var i = 0; i < validTicket.Length; i++)
                {
                    var number = validTicket[i];

                    potentialFieldsByTicketPosition[i].RemoveAll(name => Ranges[name].None(range => range.Contains(number)));
                }
            }

            while (potentialFieldsByTicketPosition.Any(fields => fields.Count > 1))
            {
                var fixedFields = potentialFieldsByTicketPosition.Where(fields => fields.Count == 1).SelectMany(name => name);

                foreach (var fields in potentialFieldsByTicketPosition.Where(fields => fields.Count > 1))
                {
                    fields.RemoveAll(name => fixedFields.Contains(name));
                }
            }

            return potentialFieldsByTicketPosition
               .SelectMany(name => name)
               .Select((name, i) => name.StartsWith("departure") ? Ticket[i] : 1L)
               .Product();
        }

        internal record Range(int Low, int High)
        {
            public static Range Parse(string[] source)
            {
                return new(int.Parse(source[0]), int.Parse(source[1]));
            }

            public bool Contains(int number)
            {
                return number.IsBetweenOrEqual(Low, High);
            }
        }
    }
}