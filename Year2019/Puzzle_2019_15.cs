using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_15 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(214, 344);
        }

        private HashSet<Tile> Tiles { get; }
        private Tile Destination { get; }

        public Puzzle_2019_15(string input)
        {
            (Tiles, Destination) = GetTiles(input);
        }

        protected override object Part1()
        {
            var toProcess = new PriorityQueue<TileWithSteps>()
            {
                new(new(0, 0), 0)
            };

            var stepsByTile = new Dictionary<Tile, int>(Tiles.Count);


            while (toProcess.Count > 0)
            {
                var current = toProcess.Dequeue();

                stepsByTile[current.Tile] = current.Steps;

                if (current.Tile == Destination) break;

                var adjacentTileSteps = current.Steps + 1;

                foreach (var adjacentTile in AdjacentTiles(current.Tile))
                {
                    if (!stepsByTile.ContainsKey(adjacentTile))
                    {
                        toProcess.Add(new (adjacentTile, adjacentTileSteps));
                    }
                }
            }

            var steps = stepsByTile[Destination];

            return steps;
        }

        protected override object Part2()
        {
            var processed = new HashSet<Tile> { Destination };
            var toProcess = AdjacentTiles(Destination).ToHashSet();

            var minutes = 0;
            do
            {
                minutes++;

                processed.AddRange(toProcess);

                var toProcessNext = new HashSet<Tile>(toProcess.Count*4);

                foreach (var tile in toProcess)
                foreach (var adjacentTile in AdjacentTiles(tile))
                    if(!processed.Contains(adjacentTile))
                        toProcessNext.Add(adjacentTile);

                toProcess = toProcessNext;
            } while (toProcess.Count > 0);

            return minutes;
        }

        private static (HashSet<Tile>, Tile) GetTiles(string input)
        {
            var original = new IntcodeProgram(input);

            var queue = new Queue<(IntcodeProgram program, Tile from, long direction)>();
            queue.Enqueue((new IntcodeProgram(original), new ( 0,  1), 1));
            queue.Enqueue((new IntcodeProgram(original), new ( 0, -1), 2));
            queue.Enqueue((new IntcodeProgram(original), new (-1,  0), 3));
            queue.Enqueue((new IntcodeProgram(original), new ( 1,  0), 4));

            var tiles = new HashSet<Tile>()
            {
                new (0, 0)
            };

            var destination = new Tile(0, 0);

            while (queue.Count > 0)
            {
                var (program, to, direction) = queue.Dequeue();

                program.AddInput(direction);
                program.Step(InterruptMode.Output);
                var output = program.LastOutput;

                if (output > 0) // didn't hit wall
                {
                    tiles.Add(to);

                    if(output == 2)
                        destination = to;

                    Tile t = to with { Y = to.Y + 1};
                    if (!tiles.Contains(t))
                        queue.Enqueue((new IntcodeProgram(program), t, 1));

                    t = to with { Y = to.Y - 1 };
                    if (!tiles.Contains(t))
                        queue.Enqueue((new IntcodeProgram(program), t, 2));

                    t = to with { X = to.X - 1 };
                    if (!tiles.Contains(t))
                        queue.Enqueue((new IntcodeProgram(program), t, 3));

                    t = to with { X = to.X + 1 };
                    if (!tiles.Contains(t))
                        queue.Enqueue((new IntcodeProgram(program), t, 4));
                }
            }

            return (tiles, destination);
        }

        private IEnumerable<Tile> AdjacentTiles(Tile tile)
        {
            var t = tile with { X = tile.X + 1};
            if (Tiles.Contains(t)) yield return t;

            t = tile with { X = tile.X - 1 };
            if (Tiles.Contains(t)) yield return t;

            t = tile with { Y = tile.Y + 1 };
            if (Tiles.Contains(t)) yield return t;

            t = tile with { Y = tile.Y - 1 };
            if (Tiles.Contains(t)) yield return t;
        }

        private record Tile(long X, long Y)
        {
        }

        private record TileWithSteps(Tile Tile, int Steps) : IComparable<TileWithSteps>
        {
            public int CompareTo(TileWithSteps? other)
            {
                return Steps.CompareTo(other!.Steps);
            }
        }
    }
}