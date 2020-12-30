using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_18 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(6098, 1698);
        }

        private string Input { get; }

        public Puzzle_2019_18(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            var map = new Map(Input, false);

            // We are always going from the start or a key to a key
            // As we move from one tile to another we should always take the best route.
            // The best route will most likely require some keys to have already been visited to go though doors.
            // The following matrix captures the best routes between all the important points and their requirements
            var keyToKeyMatrix = map.KeysByLocation.ToDictionary(
                kvp => kvp.Value,
                kvp => map.GetFastedRoutesToOtherKeys(kvp.Key));

            keyToKeyMatrix['@'] = map.GetFastedRoutesToOtherKeys(map.EntranceLocations['@']);

            var toProcess = new Dictionary<(char key, SortedSet<char> route), int>(Part1Comparer.Instance)
            {
                [('@', new SortedSet<char>())] = 0
            };

            // The solution has to go through every key
            for (var i = 0; i < 26; i++)
            {
                var nodesToProcessNext = new Dictionary<(char key, SortedSet<char> route), int>(Part1Comparer.Instance);
                foreach (var ((key, route), distance) in toProcess)
                {
                    // Ignore keys already visit or routes we can't match the requirements
                    var reachableKeys = keyToKeyMatrix[key]
                       .Keys
                       .Except(route)
                       .Where(k => keyToKeyMatrix[key][k].requirements.IsSubsetOf(route))
                       .ToList();

                    foreach (var nextKey in reachableKeys)
                    {
                        var (nextKeySteps, _) = keyToKeyMatrix[key][nextKey];

                        var nextKeyDistance = distance + nextKeySteps;
                        var nextKeyRoute = new SortedSet<char>(route) { nextKey };

                        // See if there is already a node for the same key with a route similar route. SortedSets guarantee order independence
                        if (!nodesToProcessNext.TryGetValue((nextKey, nextKeyRoute), out var currentNextKeyDistance) || nextKeyDistance < currentNextKeyDistance)
                        {
                            nodesToProcessNext[(nextKey, nextKeyRoute)] = nextKeyDistance;
                        }
                    }
                }

                toProcess = nodesToProcessNext;
            }

            var bestRoute = toProcess.Values.Min();

            return bestRoute;
        }

        protected override object Part2()
        {
            var map = new Map(Input, true);

            // Just like part 1
            // But the state is the position of every robot and we only move on robot at a time between nodes
            var keyToKeyMatrix = map.KeysByLocation.ToDictionary(
                kvp => kvp.Value,
                kvp => map.GetFastedRoutesToOtherKeys(kvp.Key));

            keyToKeyMatrix['@'] = map.GetFastedRoutesToOtherKeys(map.EntranceLocations['@']);
            keyToKeyMatrix['£'] = map.GetFastedRoutesToOtherKeys(map.EntranceLocations['£']);
            keyToKeyMatrix['$'] = map.GetFastedRoutesToOtherKeys(map.EntranceLocations['$']);
            keyToKeyMatrix['%'] = map.GetFastedRoutesToOtherKeys(map.EntranceLocations['%']);

            var toProcess = new Dictionary<(List<char> keys, SortedSet<char> route), int>(Part2Comparer.Instance)
            {
                [(new List<char> { '@', '£', '$', '%' }, new SortedSet<char>())] = 0
            };

            for (var i = 0; i < 26; i++)
            {
                var nodesToProcessNext = new Dictionary<(List<char> keys, SortedSet<char> route), int>(Part2Comparer.Instance);
                foreach (var ((keys, route), distance) in toProcess)
                {
                    for (var r = 0; r < 4; r++)
                    {
                        var reachableKeys = keyToKeyMatrix[keys[r]]
                           .Keys
                           .Except(route)
                           .Where(k => keyToKeyMatrix[keys[r]][k].requirements.IsSubsetOf(route))
                           .ToList();

                        foreach (var nextKey in reachableKeys)
                        {
                            var nextKeys = new List<char>(keys) { [r] = nextKey };
                            var (nextKeySteps, _) = keyToKeyMatrix[keys[r]][nextKey];

                            var nextKeyDistance = distance + nextKeySteps;
                            var nextKeyRoute = new SortedSet<char>(route) { nextKey };

                            if (!nodesToProcessNext.TryGetValue((nextKeys, nextKeyRoute), out var currentNextKeyDistance) || nextKeyDistance < currentNextKeyDistance)
                            {
                                nodesToProcessNext[(nextKeys, nextKeyRoute)] = nextKeyDistance;
                            }
                        }
                    }
                }

                toProcess = nodesToProcessNext;
            }


            var bestRoute = toProcess.Values.Min();

            return bestRoute;
        }

        private class Map
        {
            public Dictionary<char, (int x, int y)> EntranceLocations { get; } = new();
            private HashSet<(int x, int y)> Tiles { get; } = new();
            public Dictionary<(int x, int y), char> KeysByLocation { get; } = new();
            private Dictionary<(int x, int y), char> DoorByLocation { get; } = new();

            public Map(string input, bool forPart2)
            {
                var lines = input.Lines().ToArray();

                if(forPart2)
                {
                    // DAMN STRING IMMUTABILITY...
                    var x = lines[0].Length / 2;
                    var y = lines.Length / 2;

                    var chars = lines[y-1].ToArray();
                    chars[x-1] = '@';
                    chars[x  ] = '#';
                    chars[x+1] = '£';
                    lines[y-1] = new string(chars);

                    chars = lines[y].ToArray();
                    chars[x-1] = '#';
                    chars[x  ] = '#';
                    chars[x+1] = '#';
                    lines[y] = new string(chars);

                    chars = lines[y+1].ToArray();
                    chars[x-1] = '$';
                    chars[x  ] = '#';
                    chars[x+1] = '%';
                    lines[y+1] = new string(chars);
                }

                for (var y = 0; y < lines.Length; y++)
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        var c = lines[y][x];
                        switch (c)
                        {
                            case '@':
                            case '£':
                            case '$':
                            case '%':
                                EntranceLocations[c] = (x, y);
                                Tiles.Add((x, y));
                                break;
                            case '.':
                                Tiles.Add((x, y));
                                break;
                            case { } key when char.IsLetter(key) && char.IsLower(key):
                                KeysByLocation[(x, y)] = key;
                                Tiles.Add((x, y));
                                break;
                            case { } door when char.IsLetter(door) && char.IsUpper(door):
                                DoorByLocation[(x, y)] = door;
                                Tiles.Add((x, y));
                                break;
                        }
                    }
            }

            private IEnumerable<(int x, int y)> AdjacentTiles((int x, int y) tile)
            {
                var (x, y) = tile;
                if (Tiles.Contains((x + 1, y))) yield return (x + 1, y);
                if (Tiles.Contains((x - 1, y))) yield return (x - 1, y);
                if (Tiles.Contains((x, y + 1))) yield return (x, y + 1);
                if (Tiles.Contains((x, y - 1))) yield return (x, y - 1);
            }

            public Dictionary<char, (int steps, SortedSet<char> requirements)> GetFastedRoutesToOtherKeys((int x, int y) start)
            {
                var toProcess = new List<(int, int)> { start };

                var stepsByTile = new Dictionary<(int, int), int> { [start] = 0 };
                var tileParents = new Dictionary<(int, int), (int, int)>();

                while (toProcess.Count > 0)
                {
                    var current = toProcess.MinBy(t => stepsByTile[t]).First();

                    toProcess.Remove(current);

                    var adjacentTileSteps = stepsByTile[current] + 1;

                    foreach (var adjacentTile in AdjacentTiles(current))
                    {
                        if (!stepsByTile.TryGetValue(adjacentTile, out var currentAdjacentTileSteps) || adjacentTileSteps < currentAdjacentTileSteps)
                        {
                            stepsByTile[adjacentTile] = adjacentTileSteps;
                            toProcess.Add(adjacentTile);
                            tileParents[adjacentTile] = current;
                        }
                    }
                }


                var routes = new Dictionary<char, (int, SortedSet<char>)>();
                foreach (var (tile, key) in KeysByLocation)
                {
                    if (!stepsByTile.ContainsKey(tile)) continue;
                    var steps = stepsByTile[tile];
                    if (steps == 0) continue;

                    var requirements = new SortedSet<char>();

                    var tileInPath = tile;
                    while (tileParents.TryGetValue(tileInPath, out tileInPath) && tileInPath != start)
                    {
                        if (DoorByLocation.TryGetValue(tileInPath, out var d))
                        {
                            requirements.Add(char.ToLower(d));
                        }

                        if (KeysByLocation.TryGetValue(tileInPath, out var k))
                        {
                            requirements.Add(k);
                        }
                    }

                    routes[key] = (steps, requirements);
                }

                return routes;
            }
        }

        private class Part1Comparer : IEqualityComparer<(char key, SortedSet<char> route)>
        {
            public static Part1Comparer Instance { get; } = new();

            private Part1Comparer() { }
            public bool Equals((char key, SortedSet<char> route) x, (char key, SortedSet<char> route) y)
            {
                return x.key == y.key && x.route.SetEquals(y.route);
            }

            public int GetHashCode((char key, SortedSet<char> route) obj)
            {
                var hashCode = new HashCode();

                hashCode.Add(obj.key);

                foreach (var c in obj.route)
                {
                    hashCode.Add(c);
                }

                return hashCode.ToHashCode();
            }
        }

        private class Part2Comparer : IEqualityComparer<(List<char> keys, SortedSet<char> route)>
        {
            public static Part2Comparer Instance { get; } = new();

            private Part2Comparer() { }
            public bool Equals((List<char> keys, SortedSet<char> route) x, (List<char> keys, SortedSet<char> route) y)
            {
                return x.keys[0] == y.keys[0] && x.keys[1] == y.keys[1] && x.keys[2] == y.keys[2] && x.keys[3] == y.keys[3] && x.route.SetEquals(y.route);
            }

            public int GetHashCode((List<char> keys, SortedSet<char> route) obj)
            {
                var hashCode = new HashCode();

                foreach (var c in obj.keys)
                {
                    hashCode.Add(c);
                }

                foreach (var c in obj.route)
                {
                    hashCode.Add(c);
                }

                return hashCode.ToHashCode();
            }
        }
    }
}