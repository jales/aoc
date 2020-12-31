using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Leaderboards.Model;
using Spectre.Console;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Leaderboards
{
    internal static class Statistics
    {
        public static void Render(Leaderboard leaderboard)
        {
            var rows = leaderboard.MembersByScore
               .Select(member => BuildStatisticsRow(member, leaderboard));

            var table = BuildStatisticsTable(leaderboard);
            table.AddRows(rows);

            RenderTable(table, " [ S T A T I S T I C S ] ", Color.DarkSeaGreen);
        }

        private static IEnumerable<string> BuildStatisticsRow(Member member, Leaderboard leaderboard)
        {
            // Name
            yield return MemberName(member.Name);

            // Score
            yield return member.LocalScore.ToString();

            // Minimum Score
            yield return leaderboard.MinimumScore(member.Id).ToString();

            // Maximum Score
            yield return leaderboard.MaximumScore(member.Id).ToString();

            // Part 1
            yield return HalfDay($"{member.Part1Stars}*");

            // Part 1 First/Last
            var part1Fastest = leaderboard.SolvingTimes.Part1FastestTimes(member.Id);
            var part1Slowest = leaderboard.SolvingTimes.Part1SlowestTimes(member.Id);
            yield return
                (part1Fastest > 0 ? Fastest(part1Fastest.ToString().PadLeft(2)) : Disabled(" -")) +
                " / " +
                (part1Slowest > 0 ? Slowest(part1Slowest.ToString().PadLeft(2)) : Disabled(" -"));

            // Part 2
            yield return FullDay($"{member.Part2Stars}*");

            // Part 2 First/Last
            var part2Fastest = leaderboard.SolvingTimes.Part2FastestTimes(member.Id);
            var part2Slowest = leaderboard.SolvingTimes.Part2SlowestTimes(member.Id);
            yield return
                (part2Fastest > 0 ? Fastest(part2Fastest.ToString().PadLeft(2)) : Disabled(" -")) +
                " / " +
                (part2Slowest > 0 ? Slowest(part2Slowest.ToString().PadLeft(2)) : Disabled(" -"));

            // Average Delta
            var averageDelta = leaderboard.SolvingTimes.AverageDelta(member.Id);
            yield return averageDelta.HasValue
                ? Delta(averageDelta.Value)
                : Disabled("-  ");

            // Average Points
            var avg = member.LocalScore / (double)member.TotalStars;
            yield return double.IsFinite(avg) ? avg.ToString("0.##") : Disabled("-");

            // Late Submissions
            yield return leaderboard.SolvingTimes.LateSubmissions(member.Id).ToString();

            // Last Submission
            var lastSubmission = member.LastSubmission;
            yield return lastSubmission.HasValue
                ? Time(TimeZoneInfo.ConvertTime(lastSubmission.Value, TimeZoneInfo.Local).ToString("yyyy/MM/dd HH:mm:ss"))
                : Disabled("-");
        }

        private static Table BuildStatisticsTable(Leaderboard leaderboard)
        {
            return new Table()
               .MinimalBorder()
               .Centered()
               .Collapse()
                // Name
               .AddColumn("", c => c.RightAligned().NoWrap())
                // Score
               .AddColumn("\r\nScore", c => c.Centered().NoWrap())
                // Minimum Score
               .AddColumn("Minimum\r\nScore[grey]¹[/]", c => c.Centered().NoWrap())
                // Maximum CS
               .AddColumn("Maximum\r\nScore[grey]¹[/]", c => c.Centered().NoWrap())
                // Part 1 Stars
               .AddColumn("\r\nPart 1", c => c.Centered().NoWrap())
                // Part 1 First/Last
               .AddColumn("Part 1\r\n" + Fastest("First") + " / " + Slowest("Last"), c => c.Centered().NoWrap())
                // Part 2 Stars
               .AddColumn("\r\nPart 2", c => c.Centered().NoWrap())
                // Part 2 First/Last
               .AddColumn("Part 2\r\n" + Fastest("First") + " / " + Slowest("Last"), c => c.Centered().NoWrap())
                // Average Delta
               .AddColumn("\r\nAverage " + Delta(), c => c.RightAligned().NoWrap())
                // Average Points
               .AddColumn("Average\r\nPoints", c => c.Centered().NoWrap())
                // Late Submissions
               .AddColumn("Late\r\nSubmissions", c => c.Centered().NoWrap())
                // Last Submission
               .AddColumn("Last\r\nSubmission", c => c.Centered().NoWrap())
               .Caption("¹ Assumes full completion".EscapeMarkup(), new Style(Color.Grey));
        }
    }
}