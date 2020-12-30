using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Math;

namespace AoC.Year2020
{
	internal sealed class Puzzle_2020_20 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution(18482479935793, 2118);

            AddTest(@"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...", 20899048083289, 273);
        }

        private Tile[] Tiles { get; }
        private TileConfiguration[][] PictureTiles { get; set; } = new TileConfiguration[0][];

        public Puzzle_2020_20(string input)
        {
            Tiles = input
                .ParseLineGroups(Tile.Parse);
        }

        protected override object Part1()
        {
            var unmatchableEdges = Tiles
               .SelectMany(t => t.Edges)
               .GroupBy(x => x)
               .Where(g => g.Count() == 1)
               .Select(g => g.Key)
               .ToHashSet();

            var side = (int)Sqrt(Tiles.Length);
            var tiles = Tiles.ToList();
            PictureTiles = Enumerable.Range(0, side).Select(_ => new TileConfiguration[side]).ToArray();

            for (var y = 0; y < side; y++)
            for (var x = 0; x < side; x++)
            {
                var configurations = tiles.SelectMany(t => t.Configurations)
                   .Where(tc => y == 0 ? unmatchableEdges.Contains(tc.Top) : tc.Top == PictureTiles[y - 1][x].Bottom)
                   .Where(tc => x == 0 ? unmatchableEdges.Contains(tc.Left) : tc.Left == PictureTiles[y][x - 1].Right)
                   .ToList();

                if (x == 0 && y == 0)
                {
                    configurations.RemoveAt(0);
                }

                var tileConfiguration = configurations
                   .First();

                PictureTiles[y][x] = tileConfiguration;

                tiles.RemoveAll(t => t.Id == tileConfiguration.Id);
            }

            return
                PictureTiles[0][0].Id *
                PictureTiles[0][^1].Id *
                PictureTiles[^1][0].Id *
                PictureTiles[^1][^1].Id;
        }

        protected override object Part2()
        {
            var picture = BuildPicture();

            var roughness = picture.SelectMany(c => c).Count(c => c == '#');

            foreach (var monsterPattern in MonsterPatterns())
            {
                var matches = FindMatches(picture, monsterPattern);

                if (matches > 0)
                {
                    roughness -= matches * monsterPattern.Length;
                    break;
                }
            }
            return roughness;
        }

        private char[][] BuildPicture()
        {
            var tileHeight = Tiles[0].Grid.Length - 2;
            var tileWidth = Tiles[0].Grid[0].Length - 2;

            var picture = JaggedArray<char>(PictureTiles.Length * tileHeight, PictureTiles[0].Length * tileWidth);

            for (var y = 0; y < PictureTiles.Length; y++)
            for (var x = 0; x < PictureTiles.Length; x++)
            {
                var tile = TransformTile(PictureTiles[y][x]);

                for (var yTile = 0; yTile < tile.Length; yTile++)
                for (var xTile = 0; xTile < tile[0].Length; xTile++)
                {
                    picture[y * tileHeight + yTile][x * tileWidth + xTile] = tile[yTile][xTile];
                }
            }
            return picture;
        }

        private char[][] TransformTile(TileConfiguration tileConfiguration)
        {
            var grid = Tiles.First(t => t.Id == tileConfiguration.Id).Grid;

            //RemoveBorders
            grid = grid[1..^1].Select(row => row[1..^1]).ToArray();

            foreach (var transformation in tileConfiguration.Transformations)
            {
                char[][] transformed = JaggedArray<char>(grid.Length, grid[0].Length);

                if (transformation == 'R')
                {
                    for (var y = 0; y < grid.Length; y++)
                    for (var x = 0; x < grid[0].Length; x++)
                    {
                        transformed[x][^(y+1)] = grid[y][x];
                    }
                }
                else
                {
                    for (var y = 0; y < grid.Length; y++)
                    for (var x = 0; x < grid[0].Length; x++)
                    {
                        transformed[^(y+1)][x] = grid[y][x];
                    }
                }
                grid = transformed;
            }

            return grid;
        }

        private static IEnumerable<MonsterPattern> MonsterPatterns()
        {
            // Monster pattern in the 'monster' coordinates system
            //
            // -1                  #
            //  0#    ##    ##    ###
            //  1 #  #  #  #  #  #
            //   01234567890123456789

            List<(int y, int x)> pattern = new()
            {
                (-1, 18), (0, 0), (0, 5), (0, 6), (0, 11), (0, 12), (0, 17), (0, 18), (0, 19), (1,1), (1,4), (1,7), (1,10), (1,13), (1,16)
            };

            yield return MonsterPattern.Parse(pattern);
            yield return MonsterPattern.Parse(Flip(pattern));

            pattern = Rotate(pattern);
            yield return MonsterPattern.Parse(pattern);
            yield return MonsterPattern.Parse(Flip(pattern));

            pattern = Rotate(pattern);
            yield return MonsterPattern.Parse(pattern);
            yield return MonsterPattern.Parse(Flip(pattern));

            pattern = Rotate(pattern);
            yield return MonsterPattern.Parse(pattern);
            yield return MonsterPattern.Parse(Flip(pattern));

            static List<(int y, int x)> Rotate(IEnumerable<(int y, int x)> basePattern) =>
                basePattern.Select(coords => (coords.x, - coords.y)).ToList();

            static List<(int y, int x)> Flip(IEnumerable<(int y, int x)> basePattern) =>
                basePattern.Select(coords => (-coords.y, coords.x)).ToList();
        }

        private static int FindMatches(char[][] picture, MonsterPattern monsterPattern)
        {
            var matches = 0;

            // Adjust search boundaries to ignore pixels that would transform the monster pattern
            // outside of the picture
            var yMin = 0 - monsterPattern.Top;
            var yMax = picture.Length - monsterPattern.Bottom;
            var xMin = 0 - monsterPattern.Left;
            var xMax = picture[0].Length - monsterPattern.Right;

            for (var y = yMin; y < yMax; y++)
            for (var x = xMin; x < xMax; x++)
            {
                // The monster pattern will always starts at (0, 0) coords in the 'monster' coordinates system
                // So we can ignore any pixel that is not '#'
                if (picture[y][x] != '#') continue;

                // Transform the pattern from 'monster' coordinates system to 'current pixel' coordinates system
                // and check if all corresponding pixels are '#'
                if (monsterPattern.Pattern.All(coords => picture[coords.y + y][coords.x + x] == '#'))
                {
                    matches++;
                }
            }

            return matches;
        }


        private record Tile(long Id, char[][] Grid, List<int> Edges, List<TileConfiguration> Configurations)
        {
            public static Tile Parse(string[] lines)
            {
                //               A-B             D-A   C-D   B-C
                // From a tile:  | | you can get | |   | |   | | by rotating 90° clockwise
                //               D-C             C-B   B-A   A-D
                //
                //     D-C   C-B   B-A   A-D
                // and | |   | |   | |   | | from an horizontal flip of the above four configuration
                //     A-B   D-A   C_D   B-C
                //
                // Any combination of rotations (left/right) and/or flips (vertical/horizontal) ends up on one of the
                // above eight configurations.
                //
                // A configuration can be represented by its four edges and the transformations to create it from the
                // parsed tile. For simplicity, I only use clockwise 90° rotation 'R' and horizontal flips 'F'
                //
                // For example the first configuration above can be represented by the edges:
                //  -             Top: AB
                //  -           Right: BC
                //  -          Bottom: DC
                //  -            Left: AD
                //  - Transformations: {}
                //
                // And the last configuration by:
                //  -             Top: AD
                //  -           Right: DC
                //  -          Bottom: BC
                //  -            Left: AB
                //  - Transformations: {R,R,R,F}
                //
                // Edges are a sequence of '. and '#' characters.
                // By converting '.' -> '0' and '#' -> '1' the resulting binary sequence can be converted to an integer
                // allowing edges to be compared with a single integer comparison


                var id = lines[0].Split(' ', ':').Parse(parts => parts[1].ToInt32());

                var grid = lines[1..].Select(l => l.ToArray()).ToArray();

                var edges = new Dictionary<string, int>();
                edges["AB"] = AsInt(grid[0]);
                edges["BA"] = AsInt(grid[0].Reverse());
                edges["DC"] = AsInt(grid[^1]);
                edges["CD"] = AsInt(grid[^1].Reverse());
                edges["AD"] = AsInt(grid.Select(a => a[0]));
                edges["DA"] = AsInt(grid.Select(a => a[0]).Reverse());
                edges["BC"] = AsInt(grid.Select(a => a[^1]));
                edges["CB"] = AsInt(grid.Select(a => a[^1]).Reverse());

                var configurations = new List<TileConfiguration>
                {
                    new(id, edges["AB"], edges["BC"], edges["DC"], edges["AD"], new()),

                    new(id, edges["DA"], edges["AB"], edges["CB"], edges["DC"], new() {'R'}),
                    new(id, edges["CD"], edges["DA"], edges["BA"], edges["CB"], new() {'R', 'R'}),
                    new(id, edges["BC"], edges["CD"], edges["AD"], edges["BA"], new() {'R', 'R', 'R'}),

                    new(id, edges["DC"], edges["CB"], edges["AB"], edges["DA"], new() {'F'}),
                    new(id, edges["CB"], edges["BA"], edges["DA"], edges["CD"], new() {'R', 'F'}),
                    new(id, edges["BA"], edges["AD"], edges["CD"], edges["BC"], new() {'R', 'R', 'F'}),
                    new(id, edges["AD"], edges["DC"], edges["BC"], edges["AB"], new() {'R', 'R', 'R', 'F'}),
                };

                return new Tile(id, grid, edges.Select(kvp => kvp.Value).ToList(), configurations);

                static int AsInt(IEnumerable<char> chars)
                {
                    return Convert.ToInt32(new string(chars.Select(c => c == '.' ? '0' : '1').ToArray()), 2);
                }
            }
        }

        private record TileConfiguration(long Id, int Top, int Right, int Bottom, int Left, List<char> Transformations)
        {
        }
    }

    internal record MonsterPattern(IReadOnlyList<(int y, int x)> Pattern, int Top, int Right, int Bottom, int Left, int Length)
    {
        public static MonsterPattern Parse(IReadOnlyList<(int y, int x)> pattern)
        {
            // Calculate the boundaries of the monster pattern in the 'monster' coordinates system
            var top = 0;
            var right = 0;
            var bottom = 0;
            var left = 0;
            foreach (var (y, x) in pattern)
            {
                top = Min(top, y);
                right = Max(right, x);
                bottom = Max(bottom, y);
                left = Min(left, x);
            }

            return new(pattern, top, right, bottom, left, pattern.Count);
        }
    }
}