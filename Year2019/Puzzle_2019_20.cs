using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_20 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(696, 7538);
        }

        private string Input { get; }

        public Puzzle_2019_20(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            var map = new Map(Input);

            var destination = map.Exit;

            var toProcess = new List<(int, int)> { map.Entrance };

            var stepsByTile = new Dictionary<(int, int), int> { [map.Entrance] = 0 };

            while (toProcess.Count > 0)
            {
                var current = toProcess.MinBy(t => stepsByTile[t]).First();

                if (current == destination)
                    break;

                toProcess.Remove(current);

                var adjacentTileSteps = stepsByTile[current] + 1;

                foreach (var adjacentTile in map.AdjacentTiles(current))
                {
                    if (!stepsByTile.TryGetValue(adjacentTile, out var currentAdjacentTileSteps) || adjacentTileSteps < currentAdjacentTileSteps)
                    {
                        stepsByTile[adjacentTile] = adjacentTileSteps;
                        toProcess.Add(adjacentTile);
                    }
                }
            }

            var steps = stepsByTile[destination];

            return steps;
        }

        protected override object Part2()
        {
            var map = new Map(Input);

            var destination = (map.Exit.x, map.Exit.y, 0);

            var mapEntrance = (map.Entrance.x, map.Entrance.y, 0);
            var toProcess = new List<(int, int, int)> { mapEntrance };

            var stepsByTile = new Dictionary<(int, int, int), int> { [mapEntrance] = 0 };

            while (toProcess.Count > 0)
            {
                var current = toProcess.MinBy(t => stepsByTile[t]).First();

                if (current == destination)
                    break;

                toProcess.Remove(current);

                var adjacentTileSteps = stepsByTile[current] + 1;

                foreach (var adjacentTile in map.AdjacentTiles(current))
                {
                    if (!stepsByTile.TryGetValue(adjacentTile, out var currentAdjacentTileSteps) || adjacentTileSteps < currentAdjacentTileSteps)
                    {
                        stepsByTile[adjacentTile] = adjacentTileSteps;
                        toProcess.Add(adjacentTile);
                    }
                }
            }

            var steps = stepsByTile[destination];

            return steps;
        }

        private class Map
        {
            private HashSet<(int x, int y)> Tiles { get; } = new();
            public (int x, int y) Entrance { get; }
            public (int x, int y) Exit { get; }
            private Dictionary<(int x, int y), string> PortalLocations { get; } = new();
            private HashSet<(int x, int y)> OuterPortals { get; } = new();
            private Dictionary<string, ((int x, int y) entry1, (int x, int y) entry2)> PortalPairs { get; } = new Dictionary<string, ((int x, int y) e1, (int x, int y) e2)>();

            public Map(string input)
            {
                var lines = input.Lines().ToArray();

                for (var y = 0; y < lines.Length; y++)
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        var c = lines[y][x];
                        if (c != '.') continue;
                        Tiles.Add((x, y));

                        if (char.IsLetter(lines[y][x - 2]) && char.IsLetter(lines[y][x - 1]))
                        {
                            PortalLocations[(x, y)] = new string(new[] { lines[y][x - 2], lines[y][x - 1] });
                            if (x == 2) OuterPortals.Add((x, y));
                        }

                        if (char.IsLetter(lines[y][x + 1]) && char.IsLetter(lines[y][x + 2]))
                        {
                            PortalLocations[(x, y)] = new string(new[] { lines[y][x + 1], lines[y][x + 2] });
                            if (x == lines[y].Length - 3) OuterPortals.Add((x, y));
                        }

                        if (char.IsLetter(lines[y - 2][x]) && char.IsLetter(lines[y - 1][x]))
                        {
                            PortalLocations[(x, y)] = new string(new[] { lines[y - 2][x], lines[y - 1][x] });
                            if (y == 2) OuterPortals.Add((x, y));
                        }

                        if (char.IsLetter(lines[y + 1][x]) && char.IsLetter(lines[y + 2][x]))
                        {
                            PortalLocations[(x, y)] = new string(new[] { lines[y + 1][x], lines[y + 2][x] });
                            if (y == lines.Length - 3) OuterPortals.Add((x, y));
                        }
                    }

                foreach (var (tile, portal) in PortalLocations)
                {
                    if (portal == "AA") Entrance = tile;
                    if (portal == "ZZ") Exit = tile;
                }

                foreach (var pairs in PortalLocations.GroupBy(kvp => kvp.Value).Where(g => g.Count() == 2))
                {
                    PortalPairs[pairs.Key] = (pairs.ElementAt(0).Key, pairs.ElementAt(1).Key);
                }
            }

            public IEnumerable<(int x, int y)> AdjacentTiles((int x, int y) tile)
            {
                var (x, y) = tile;
                if (PortalLocations.TryGetValue(tile, out var portal) && PortalPairs.TryGetValue(portal, out var pair))
                {
                    yield return pair.entry1 == tile ? pair.entry2 : pair.entry1;
                }

                if (Tiles.Contains((x + 1, y))) yield return (x + 1, y);
                if (Tiles.Contains((x - 1, y))) yield return (x - 1, y);
                if (Tiles.Contains((x, y + 1))) yield return (x, y + 1);
                if (Tiles.Contains((x, y - 1))) yield return (x, y - 1);
            }

            public IEnumerable<(int x, int y, int level)> AdjacentTiles((int x, int y, int level) location)
            {
                var level = location.level;
                var tile = (location.x, location.y);
                var (x, y) = tile;

                if (PortalLocations.TryGetValue(tile, out var portal) && PortalPairs.TryGetValue(portal, out var pair))
                {
                    if (level != 0 || !OuterPortals.Contains(tile))
                    {
                        var nextPortal = pair.entry1 == tile ? pair.entry2 : pair.entry1;

                        var nextLevel = OuterPortals.Contains(nextPortal) ? level - 1 : level + 1;

                        yield return (nextPortal.x, nextPortal.y, nextLevel);
                    }
                }

                if (Tiles.Contains((x + 1, y))) yield return (x + 1, y, level);
                if (Tiles.Contains((x - 1, y))) yield return (x - 1, y, level);
                if (Tiles.Contains((x, y + 1))) yield return (x, y + 1, level);
                if (Tiles.Contains((x, y - 1))) yield return (x, y - 1, level);
            }
        }
    }
}