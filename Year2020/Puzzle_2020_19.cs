using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
	internal sealed class Puzzle_2020_19 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(142, 294);

            AddPart1Test(@"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb", 2);
            AddPart2Test(@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba", 12);
        }

        private string[] Messages { get;  }
        private Dictionary<int, string> Rules { get; }

        public Puzzle_2020_19(string input)
        {
            var groups = input.LineGroups().ToArray();

            Messages = groups[1].ToArray();

            Rules = groups[0]
               .Select(l => l.Split(':'))
               .ToDictionary(x => int.Parse(x[0]), x => x[1].Replace("\"", "") + " ");
        }

        protected override object Part1()
        {
            var expression = BuildPattern();

            return Messages.Count(m => m.IsMatch(expression));
        }

        protected override object Part2()
        {
            var previousCount = 0;
            for (var i = 1; i < 100; i++)
            {
                Rules[8] = BuildRecursiveRule(" 42 | 42 8 ", "8", i);
                Rules[11] = BuildRecursiveRule(" 42 31 | 42 11 31 ", "11", i);

                var expression = BuildPattern();
                var count = Messages.Count(m => m.IsMatch(expression));

                if (count == previousCount) return count;

                previousCount = count;
            }


            return -1;
        }

        private string BuildPattern()
        {
            var rules = new Dictionary<int, string>(Rules);
            var parsedRules = new Dictionary<int, string>();

            while (true)
            {
                var parseableRules = rules.Where(r => !r.Value.HasMatch(@"\d+")).ToList();

                foreach (var (id , rule) in parseableRules)
                {
                    var parsedRule = rule.Replace(" ", "");

                    if (id == 0) return parsedRule;

                    if (parsedRule.Contains('|')) parsedRule = "(" + parsedRule + ")";

                    rules.Remove(id);
                    parsedRules[id] = parsedRule;

                    foreach (var i in rules.Keys)
                    {
                        rules[i] = Regex.Replace(rules[i], @"\d+", m => parsedRules.GetOrDefault(int.Parse(m.Value), m.Value)!);
                    }
                }
            }
        }

        private static string BuildRecursiveRule(string recursiveRule, string recursivePart, int iterations)
        {
            var rule = recursiveRule;

            for (var i = 0; i < iterations; i++)
            {
                rule = rule.Replace(recursivePart, "(" + recursiveRule + ")");
            }

            return rule.Replace(recursivePart, "");
        }
    }
}