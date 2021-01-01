using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using BenchmarkDotNet.Reports;
using Spectre.Console;
using static System.Linq.Enumerable;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Benchmarks
{
    internal static class Statistics
    {
        public static void Render(PuzzleRegistration registration, Summary summary)
        {
            var rows = Range(0, summary.BenchmarksCases.Length)
            .Select(index => BuildStatisticsRow(index, summary));

            var table = BuildStatisticsTable(summary);
            table.AddRows(rows);

            RenderTable(table, BuildHeader(registration), Color.Blue);
        }

        private static string BuildHeader(PuzzleRegistration registration)
        {
            return " [ S T A T I S T I C S   " +
                string.Join(" ", registration.Year.ToString().ToCharArray()) +
                " / " +
                string.Join(" ", registration.Day.ToString().ToCharArray()) +
                " ] ";
        }

        private static IEnumerable<string> BuildStatisticsRow(int index, Summary summary)
        {
            yield return index switch
            {
                0 => MemberName("Input parsing"),
                1 => MemberName("Solving part 1"),
                _ => MemberName("Solving part 2"),
            };

            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "Min").Content[index]));
            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "Mean").Content[index]));
            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "P95").Content[index]));
            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "Max").Content[index]));
            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "Error").Content[index]));
            yield return Time(ParseNanoseconds(summary.Table.Columns.First(c => c.Header == "StdDev").Content[index]));

            yield return Memory(ParseBytes(summary.Table.Columns.First(c => c.Header == "Allocated").Content[index]));
            yield return GCCount(ParseGCCount(summary.Table.Columns.First(c => c.Header == "Gen 0").Content[index]));
            yield return GCCount(ParseGCCount(summary.Table.Columns.First(c => c.Header == "Gen 1").Content[index]));
            yield return GCCount(ParseGCCount(summary.Table.Columns.First(c => c.Header == "Gen 2").Content[index]));

            yield return summary.Table.Columns.First(c => c.Header == "IterationCount").Content[index];
        }

        private static Table BuildStatisticsTable(Summary summary)
        {
            var table = new Table()
               .MinimalBorder()
               .Centered()
               .Collapse()
               .AddColumn("", c => c.RightAligned())
               .AddColumn("\r\nMin", c => c.RightAligned())
               .AddColumn("\r\nMean", c => c.RightAligned())
               .AddColumn("\r\nP95", c => c.RightAligned())
               .AddColumn("\r\nMax", c => c.RightAligned())
               .AddColumn("\r\nError", c => c.RightAligned())
               .AddColumn("\r\nStd Dev", c => c.RightAligned())
               .AddColumn("\r\nAllocated", c => c.RightAligned())
               .AddColumn("\r\nGen 0", c => c.RightAligned())
               .AddColumn("\r\nGen 1", c => c.RightAligned())
               .AddColumn("\r\nGen 2", c => c.RightAligned())
               .AddColumn("Iteration\r\nCount", c => c.Centered());

            if (summary.ValidationErrors.Length > 0)
            {
                table.Caption(
                    "• " + string.Join(Environment.NewLine + "• ", summary.ValidationErrors.Select(e => e.Message)),
                    new Style(Color.Orange4));
            }

            return table;
        }
    }
}
