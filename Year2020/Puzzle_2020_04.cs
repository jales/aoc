using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_04 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(264, 224);

            AddTest(@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in", 2, 2);
        }


        private List<Dictionary<string, string>> Passports { get; }

        public Puzzle_2020_04(string input)
        {
            Passports = input
                .ParseLineGroups(lines => lines
                    .SelectMany(line => line.Split(' '))
                    .Select(field => field.Split(':'))
                    .ToDictionary(p => p[0], p => p[1]))
                .ToList();
        }

        protected override object Part1()
        {
            return Passports.Count(p =>
                p.ContainsKey("byr") &&
                p.ContainsKey("iyr") &&
                p.ContainsKey("eyr") &&
                p.ContainsKey("hgt") &&
                p.ContainsKey("hcl") &&
                p.ContainsKey("ecl") &&
                p.ContainsKey("pid"));
        }

        protected override object Part2()
        {
            // language=regex
            return Passports.Count(p =>
                p.TryGetValue("byr", int.Parse, out var byr) && byr.IsBetweenOrEqual(1920, 2020) &&

                p.TryGetValue("iyr", int.Parse, out var iyr) && iyr.IsBetweenOrEqual(2010, 2020) &&

                p.TryGetValue("eyr", int.Parse, out var eyr) && eyr.IsBetweenOrEqual(2020, 2030) &&

                p.TryGetValue("hgt", v => v.MatchesByName("(?<value>[0-9]+)(?<unit>cm|in)") , out var hgt) &&
                (
                    (hgt.GetOrDefault("unit"), hgt.GetOrDefault("value", int.Parse)) switch
                    {
                        ("cm", var x) => x.IsBetweenOrEqual(150, 193),
                        ("in", var x) => x.IsBetweenOrEqual(59, 76),
                        _ => false
                    }
                ) &&

                p.TryGetValue("hcl", out var hcl) && hcl.IsMatch("#[0-9a-f]{6}") &&

                p.TryGetValue("ecl", out var ecl) && ecl.IsMatch("amb|blu|brn|gry|grn|hzl|oth") &&

                p.TryGetValue("pid", out var pid) && pid.IsMatch("[0-9]{9}"));
        }
    }
}