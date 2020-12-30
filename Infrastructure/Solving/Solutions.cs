using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using static AoC.Infrastructure.Services.AdventOfCode;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Solving
{
    internal static class Solutions
    {
        public static void Render(Dictionary<(int year, int day), int> solutions)
        {
            var rows = GetYears()
               .Select(year => BuildSolutionsRow(solutions, year));

            var table = BuildSolutionsTable();
            table.AddRows(rows);

            RenderTable(table, " [ S O L U T I O N S   L I S T ] ", Color.Blue);
        }

        private static IEnumerable<string> BuildSolutionsRow(IDictionary<(int year, int day), int> solutions, int year)
        {
            // Year
            yield return Year(year);

            // Days
            yield return string.Join(" ", GetDays().Select(day => solutions.GetOrDefault((year, day), 0) switch
            {
                2 => FullDay("*"),
                1 => HalfDay("*"),
                _ => " "
            }));
        }

        private static Table BuildSolutionsTable()
        {
            return new Table()
               .SimpleBorder()
               .Centered()
               .Collapse()
                // Year
               .AddColumn("", c => c.RightAligned().NoWrap())
                // Days
               .AddColumn(GetDaysColumnHeader(), c => c.Width(51).NoWrap());

            string GetDaysColumnHeader()
            {
                var days = GetDays()
                   .Select(day => new[]
                    {
                        Day(day / 10),
                        Day(day % 10)
                    })
                   .ToList();

                return string.Join(Environment.NewLine,
                    string.Join(" ", days.Select(d => d[0])),
                    string.Join(" ", days.Select(d => d[1])));
            }

        }
    }
}