using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using Spectre.Console;

namespace AoC.Infrastructure.Services
{
    internal static class Renderer
    {
        public static string Day(int day) => $"[lime]{day}[/]";
        public static string Year(int year) => $"[lime]{year}[/]";

        public static string Disabled(int value) => $"[grey27]{value}[/]";
        public static string Disabled(string value) => $"[grey27]{value}[/]";

        public static string MemberName(string name) => $"[green]{name}[/]";

        public static string FullDay(string str) => $"[yellow]{str}[/]";
        public static string HalfDay(string str) => $"[steelblue]{str}[/]";

        public static string Fastest(string value) => $"[darkseagreen4]{value}[/]";
        public static string Slowest(string value) => $"[hotpink3]{value}[/]";
        public static string Time(string value) => $"[steelblue]{value}[/]";

        public static int RenderedLength(TimeSpan? value)
        {
            return !value.HasValue
                ? 5
                : value.Value.Days > 0
                    ? 10 + (int)Math.Floor(Math.Log10(value.Value.Days))
                    : value.Value.Hours > 0
                        ? 8
                        : 5;
        }

        public static Table AddColumns(this Table source, IEnumerable<string> columns, Action<TableColumn>? configure = null)
        {
            foreach (var column in columns)
            {
                source.AddColumn(column, configure);
            }

            return source;
        }

        public static Table AddRows(this Table source, IEnumerable<IEnumerable<string>> rows)
        {
            foreach (var row in rows)
            {
                source.AddRow(row.ToArray());
            }

            return source;
        }

        public static async Task<T> Status<T>(string status, Func<Task<T>> function)
        {
            T result = default;

            await AnsiConsole.Status()
               .Spinner(Spinner.Known.Dots8)
               .SpinnerStyle(Style.Parse("blue bold"))
               .StartAsync(status, async _ =>
                {
                    result = await function();
                });

            return result!;
        }

        public static async Task Status(string status, Func<Task> function)
        {
            await AnsiConsole.Status()
               .Spinner(Spinner.Known.Dots8)
               .SpinnerStyle(Style.Parse("blue bold"))
               .StartAsync(status, async _ => await function());
        }

        public static string MemberTime(TimeSpan? time, TimeSpan? fastest, TimeSpan? slowest, int size)
        {
            if (!time.HasValue) return Disabled(NoTime(size));

            var value = time.Value;
            if (time == fastest) return Fastest(Time(value, size));
            if (time == slowest) return Slowest(Time(value, size));
            return Time(Time(value, size));
        }

        public static string MemberDelta(TimeSpan? time, TimeSpan? fastest, TimeSpan? slowest, int size)
        {
            if (!time.HasValue) return Disabled(NoTime(size + 2));

            var value = time.Value;
            if (time == fastest) return Delta(Fastest(Time(value, size)));
            if (time == slowest) return Delta(Slowest(Time(value, size)));
            return Delta(Time(Time(value, size)));
        }

        public static string Delta() => $"[fuchsia]∆[/]";

        public static string Delta(TimeSpan v) => Delta(Time(Time(v, 1)));

        private static string Delta(string value) => $"{value} [fuchsia]∆[/]";

        private static string Time(TimeSpan value, int size)
        {
            return value.Days > 0
                ? value.ToString("d\\.hh\\:mm\\:ss").PadLeft(size)
                : value.Hours > 0
                    ? value.ToString("hh\\:mm\\:ss").PadLeft(size)
                    : value.ToString("mm\\:ss").PadLeft(size);
        }

        private static string NoTime(int size) => "-  ".PadLeft(size);

        public static void RenderTable(Table table, string header, Color color)
        {
            AnsiConsole.Render(new Panel(table)
               .Header($"[{color}]" + header.EscapeMarkup() + "[/]", Justify.Center)
               .RoundedBorder()
               .BorderStyle(new Style(color))
               .Expand());
        }

        public static void PuzzleHeader(int year, int day)
        {
            var header = $"── Year {year} day {day} ".PadRight(76, '─');
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[deepskyblue1]{header}[/]");
            AnsiConsole.WriteLine();
        }

        public static void PuzzleFooter()
        {
            var footer = new string('─', 76);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[deepskyblue1]{footer}[/]");
            AnsiConsole.WriteLine();
        }

        public static void RenderPuzzleResult(PuzzleResult puzzleResult)
        {
            if (puzzleResult.HasNoResults()) return;

            if (puzzleResult.ResultIsExpected())
            {
                RenderPuzzleResultTitle(puzzleResult);
                RenderPuzzleSuccess(puzzleResult);
                RenderPuzzleElapsedTime(puzzleResult);
                RenderPuzzleLogs(puzzleResult);
            }
            else if (puzzleResult.ResultIsUnknown())
            {
                RenderPuzzleResultTitle(puzzleResult);
                RenderPuzzleUnknown(puzzleResult);
                RenderPuzzleElapsedTime(puzzleResult);
                RenderPuzzleLogs(puzzleResult);
            }
            else if (puzzleResult.HasErrors())
            {
                RenderPuzzleResultTitle(puzzleResult);
                RenderPuzzleError(puzzleResult);
                RenderPuzzleElapsedTime(puzzleResult);
                RenderPuzzleLogs(puzzleResult);
            }
            else
            {
                RenderPuzzleResultTitle(puzzleResult);
                RenderPuzzleFailure(puzzleResult);
                RenderPuzzleElapsedTime(puzzleResult);
                RenderPuzzleLogs(puzzleResult);
            }
        }

        private static void RenderPuzzleResultTitle(PuzzleResult puzzleResult)
        {
            if(puzzleResult.IsPart1Result)
                AnsiConsole.Markup($"[mediumpurple]{puzzleResult.Title,6} Part 1: [/]");
            else
                AnsiConsole.Markup($"[mediumturquoise]{puzzleResult.Title,6} Part 2: [/]");
        }

        private static void RenderPuzzleSuccess(PuzzleResult puzzleResult)
        {
            if (puzzleResult.Result as string == Puzzle.MerryChristmas)
            {
                AnsiConsole.Markup("[red]M [/]");
                AnsiConsole.Markup("[orange1]E [/]");
                AnsiConsole.Markup("[yellow]R [/]");
                AnsiConsole.Markup("[green]R [/]");
                AnsiConsole.Markup("[aqua]Y   [/]");
                AnsiConsole.Markup("[blue]C [/]");
                AnsiConsole.Markup("[violet]H [/]");
                AnsiConsole.Markup("[red]R [/]");
                AnsiConsole.Markup("[orange1]I [/]");
                AnsiConsole.Markup("[yellow]S [/]");
                AnsiConsole.Markup("[green]T [/]");
                AnsiConsole.Markup("[aqua]M [/]");
                AnsiConsole.Markup("[blue]A [/]");
                AnsiConsole.Markup("[violet]S[/]");
                AnsiConsole.Write("                ");
            }
            else
            {
                var description = $"\uf00c {puzzleResult.Result}";

                if (description.Length > 45)
                    description = description[..43] + "… ";

                AnsiConsole.Markup($"[green]{description,-45}[/]");
            }
        }

        private static void RenderPuzzleUnknown(PuzzleResult puzzleResult)
        {
            var description = $"\u2049 {puzzleResult.Result} ";

            if (description.Length > 45)
                    description = description[..43] + "… ";

            AnsiConsole.Markup($"[yellow]{description,-45}[/]");
        }

        private static void RenderPuzzleFailure(PuzzleResult puzzleResult)
        {
            var expected = puzzleResult.IsPart1Result ? puzzleResult.Run.Part1Solution : puzzleResult.Run.Part2Solution;
            var description = $"\uf00d {puzzleResult.Result} (Expected {expected}) ";

            if (description.Length > 45)
                    description = description[..43] + "… ";

            AnsiConsole.Markup($"[red]{description,-45}[/]");
        }

        private static void RenderPuzzleError(PuzzleResult puzzleResult)
        {
            var description = $"\ufb8a {puzzleResult.ErrorDescription} ";

            if (description.Length > 45)
                    description = description[..43] + "… ";

            AnsiConsole.Markup($"[red]{description,-45}[/]");
        }

        private static void RenderPuzzleElapsedTime(PuzzleResult puzzleResult)
        {
            var elapsed = puzzleResult.ElapsedTime;
            if(elapsed.TotalSeconds > 1)
            {
                AnsiConsole.MarkupLine($"[fuchsia][[\ufa1a {elapsed.TotalSeconds,8:0.000} s ]][/]");
            }
            else if(elapsed.TotalMilliseconds > 1)
            {
                AnsiConsole.MarkupLine($"[fuchsia][[\ufa1a {elapsed.TotalMilliseconds,8:0.000} ms]][/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[fuchsia][[\ufa1a {elapsed.TotalMilliseconds*1000,8:0.000} µs]][/]");
            }
        }

        private static void RenderPuzzleLogs(PuzzleResult puzzleResult)
        {
            if (puzzleResult.Logs.Length > 0)
            {
                foreach (var logLine in puzzleResult.Logs.ToString().Lines(StringSplitOptions.None))
                {
                    AnsiConsole.MarkupLine($"[aqua] > [/][grey]{logLine.EscapeMarkup()}[/]");
                }
            }
        }

        public static void RenderSubmissionResponse(string response)
        {
            var firstSentence = response.Substring(0, response.IndexOfAny(new []{'.', '!', '?' }));

            switch (firstSentence.ToLower())
            {
                case "that's the right answer":
                    AnsiConsole.MarkupLine($"               [yellow]↳[/] [green]{firstSentence}[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine($"               [yellow]↳[/] [red]{firstSentence}[/]");
                    break;
            }
        }

        public static void RenderCleanupResults(int count)
        {
            if(count == 0)
                AnsiConsole.WriteLine("Local puzzle input cache was empty.");
            else if(count == 1)
                AnsiConsole.WriteLine("Removed 1 entry from the local puzzle input cache.");
            else
                AnsiConsole.WriteLine($"Removed {count} entries from the local puzzle input cache.");
        }
    }
}