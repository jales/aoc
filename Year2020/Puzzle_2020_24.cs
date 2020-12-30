using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

#pragma warning disable 8509

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_24 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution(287, 3636);

            AddTest(@"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew", 10, 2208);
        }

        private HashSet<(int, int)> Floor { get; set; } = new();
        private string[][] TileInstructions { get; }


        public Puzzle_2020_24(string input)
        {
            TileInstructions = input.ParseLines(
                line => line.Matches(@"e|w|se|sw|ne|nw"));
        }

        protected override object Part1()
        {
            foreach (var instructions in TileInstructions)
            {
                var (x, y) = (0, 0);

                foreach (var instruction in instructions)
                {
                    (x, y) = instruction switch
                    {
                        "nw" => (x - 1, y - 1),
                        "ne" => (x + 1, y - 1),
                        "e"  => (x + 2, y    ),
                        "se" => (x + 1, y + 1),
                        "sw" => (x - 1, y + 1),
                        "w"  => (x - 2, y    )
                    };
                }

                if (Floor.Contains((x, y)))
                {
                    Floor.Remove((x, y));
                }
                else
                {
                    Floor.Add((x, y));
                }
            }

            return Floor.Count;
        }

        protected override object Part2()
        {
            for (var day = 0; day < 100; day++)
            {
                Floor = Floor
                   .SelectMany(tile => Neighbors(tile).Prepend(tile)).ToList()
                   .Where(tile =>
                    {
                        var count = Neighbors(tile).Count(n => Floor.Contains(n));
                        return count == 2 || (count == 1 && Floor.Contains(tile));
                    })
                   .ToHashSet();
            }

            return Floor.Count;

            static IEnumerable<(int, int)> Neighbors((int, int) tile)
            {
                var (x, y) = tile;
                yield return (x - 1, y - 1);
                yield return (x + 1, y - 1);
                yield return (x + 2, y    );
                yield return (x + 1, y + 1);
                yield return (x - 1, y + 1);
                yield return (x - 2, y    );
            }
        }
    }
}