using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC.Infrastructure.Puzzles
{
    public static partial class PuzzleRegistrar
    {
        static PuzzleRegistrar()
        {
            foreach (var type in Assembly.GetCallingAssembly().DefinedTypes)
            {
                if (type.BaseType == typeof(Puzzle)) Register(type);
            }
        }

        private static void Register(Type puzzleType)
        {
            var match = Regex.Match(puzzleType.Name, "^Puzzle_(?<year>[0-9]{4})_(?<day>[0-9]{1,2})");
            var year = int.Parse(match.Groups["year"].Value);
            var day = int.Parse(match.Groups["day"].Value);

            var configure = Expression
               .Lambda<Action>(
                    Expression.Call(puzzleType.GetMethod("Configure")!))
               .Compile();

            var arg = Expression.Parameter(typeof(string), "input");
            var factory = Expression
               .Lambda<Func<string, Puzzle>>(
                    Expression.New(puzzleType.GetConstructor(new[] { typeof(string) })!, arg), arg)
               .Compile();

            Registrations[(year, day)] = new(puzzleType, year, day, configure, factory);
        }
    }
}