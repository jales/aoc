using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    internal sealed class Puzzle_2016_04 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(137896, 501);

            AddPart1Test(@"aaaaa-bbb-z-y-x-123[abxyz]
a-b-c-d-e-f-g-h-987[abcde]
not-a-real-room-404[oarel]
totally-real-room-200[decoy]" , 1514);
        }

        private Room[] Rooms { get; }

        public Puzzle_2016_04(string input)
        {
            Rooms = input
                .ParseLines(line =>
                {
                    var parts = line.Split(new[] { '-', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    return new Room(parts[.. ^2], int.Parse(parts[^2]), parts[^1]);
                });
        }

        protected override object Part1()
        {
            return Rooms.Where(IsValid).Sum(r => r.Id);
        }

        protected override object Part2()
        {
            var rooms = Rooms
                .Where(IsValid)
                .Select(Decrypt)
                .Where(r => r.RealName!.Contains("storage"))
                .ToList();

            return rooms.FirstOrDefault(r => r.RealName == "northpole object storage")?.Id ?? -1;
        }

        private Room Decrypt(Room room)
        {
            var realNames = room.Names
                .Select(name => new string(name.Select(c =>
                {
                    var i = c - 'a';
                    i += room.Id;
                    i %= 26;
                    return (char)('a' + i);
                }).ToArray()))
                .ToList();

            room.RealName = string.Join(" ", realNames);
            return room;
        }

        private bool IsValid(Room room)
        {
            var histogram = room.Names.Concat()
                .GroupBy(c=>c)
                .Select(c => (c.Key, Count: c.Count()))
                .OrderByDescending(c => c.Count)
                .ThenBy(c =>c.Key)
                .ToArray();

            if(histogram.Length < room.Checksum.Length) return false;

            for (int i = 0; i < room.Checksum.Length; i++)
            {
                if(room.Checksum[i] != histogram[i].Key) return false;
            }
            return true;
        }

        private record Room(string[] Names, int Id, string Checksum)
        {
            public string? RealName { get; set; }
        }
    }
}