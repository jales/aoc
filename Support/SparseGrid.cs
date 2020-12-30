using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Support
{
    public static class SparseGrid
    {
        public static SparseGrid<T> Parse<T>(string description, Func<int, int, char, (bool exists, T value)> cellParser) where T: notnull
        {
            var cells = description
               .Lines()
               .SelectMany((line, y) => line.Select((c, x) => (x, y, cell: cellParser(x, y, c))))
               .Where(result => result.cell.exists)
               .ToDictionary(result => (result.x, result.y), result => result.cell.value);

            return new SparseGrid<T>(cells);


        }

        public static SparseGrid<T> Parse<T>(string description, Func<char, (bool, T)> cellParser) where T: notnull
        {
            return Parse(description, (_, _, c) => cellParser(c));
        }
    }

    public enum SparseGridNeighbors { All, Cardinal }

    public class SparseGrid<T> : IEquatable<SparseGrid<T>> where T : notnull
    {
        public Dictionary<(int x, int y), T> Cells { get; }
        public int Height { get; }
        public int Width { get; }

        public SparseGrid(Dictionary<(int x, int y), T> cells)
        {
            Cells = cells;
            Width = cells.Keys.Max(k => k.x) + 1;
            Height = cells.Keys.Max(k => k.y) + 1;
        }

        protected SparseGrid(Dictionary<(int x, int y), T> cells, int width, int height)
        {
            Cells = cells;
            Width = width;
            Height = height;
        }

        public SparseGrid<T> Evolve(Func<int, int, T, T> selector)
        {
            return new(
                Cells.ToDictionary(kvp => kvp.Key, kvp => selector(kvp.Key.x, kvp.Key.y, kvp.Value)),
                Width,
                Height);
        }

        public SparseGrid<T> Evolve(Func<int, int, T> selector)
        {
            return new(
                Cells.ToDictionary(kvp => kvp.Key, kvp => selector(kvp.Key.x, kvp.Key.y)),
                Width,
                Height);
        }

        public void LogTo(StringBuilder log, char emptyCell, Func<T, char> selector)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    log.Append(Cells.TryGetValue((x, y), out var c) ? selector(c) : emptyCell);
                }
                log.AppendLine();
            }
            log.AppendLine();
        }

        public IEnumerable<(int x, int y, T value)> Neighbors(int cx, int cy, SparseGridNeighbors mode = SparseGridNeighbors.All)
        {
            // Clockwise
            if (mode == SparseGridNeighbors.All)
            {
                if (Cells.TryGetValue((cx    , cy - 1), out var n1)) yield return (cx    , cy - 1, n1);
                if (Cells.TryGetValue((cx + 1, cy - 1), out var n2)) yield return (cx + 1, cy - 1, n2);
                if (Cells.TryGetValue((cx + 1, cy    ), out var n3)) yield return (cx + 1, cy    , n3);
                if (Cells.TryGetValue((cx + 1, cy + 1), out var n4)) yield return (cx + 1, cy + 1, n4);
                if (Cells.TryGetValue((cx    , cy + 1), out var n5)) yield return (cx    , cy + 1, n5);
                if (Cells.TryGetValue((cx - 1, cy + 1), out var n6)) yield return (cx - 1, cy + 1, n6);
                if (Cells.TryGetValue((cx - 1, cy    ), out var n7)) yield return (cx - 1, cy    , n7);
                if (Cells.TryGetValue((cx - 1, cy - 1), out var n8)) yield return (cx - 1, cy - 1, n8);
            }
            else
            {
                if (Cells.TryGetValue((cx    , cy - 1), out var n1)) yield return (cx    , cy - 1, n1);
                if (Cells.TryGetValue((cx + 1, cy    ), out var n2)) yield return (cx + 1, cy    , n2);
                if (Cells.TryGetValue((cx    , cy + 1), out var n3)) yield return (cx    , cy + 1, n3);
                if (Cells.TryGetValue((cx - 1, cy    ), out var n4)) yield return (cx - 1, cy    , n4);
            }
        }

        public IEnumerable<(int x, int y, T value)> SlopeFrom(int cx, int cy, int dx, int dy)
        {
            var y = cy + dy;
            var x = cx + dx;

            while (y >= 0 && y < Height && x >= 0 && x < Width)
            {
                if (Cells.TryGetValue((x, y), out var value))
                    yield return (x, y, value);

                y += dy;
                x += dx;
            }
        }

        public IEnumerable<IEnumerable<(int x, int y, T value)>> SlopesFrom(int cx, int cy, SparseGridNeighbors mode = SparseGridNeighbors.All)
        {
            // Clockwise
            if (mode == SparseGridNeighbors.All)
            {
                yield return SlopeFrom(cx, cy,  0, -1);
                yield return SlopeFrom(cx, cy, +1, -1);
                yield return SlopeFrom(cx, cy, +1,  0);
                yield return SlopeFrom(cx, cy, +1, +1);
                yield return SlopeFrom(cx, cy,  0, +1);
                yield return SlopeFrom(cx, cy, -1, +1);
                yield return SlopeFrom(cx, cy, -1,  0);
                yield return SlopeFrom(cx, cy, -1, -1);
            }
            else
            {
                yield return SlopeFrom(cx, cy,  0, -1);
                yield return SlopeFrom(cx, cy, +1,  0);
                yield return SlopeFrom(cx, cy,  0, +1);
                yield return SlopeFrom(cx, cy, -1,  0);
            }
        }

        public int Count(Func<int, int, T, bool> selector)
        {
            return Cells.Count(kvp => selector(kvp.Key.x, kvp.Key.y, kvp.Value));
        }

        public int Count(Func<T, bool> selector)
        {
            return Cells.Count(kvp => selector(kvp.Value));
        }

        public bool Equals(SparseGrid<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if(Height != other.Height || Width != other.Width) return false;

            var comparer = EqualityComparer<T>.Default;

            foreach (var key in Cells.Keys)
            {
                if (!comparer.Equals(Cells[key], other.Cells[key])) return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SparseGrid<T>) obj);
        }

        public override int GetHashCode()
        {
            // I know it's wrong!
            return HashCode.Combine(Cells, Height, Width);
        }

        public static bool operator ==(SparseGrid<T>? left, SparseGrid<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SparseGrid<T>? left, SparseGrid<T>? right)
        {
            return !Equals(left, right);
        }
    }
}
