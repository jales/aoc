using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    public sealed class Puzzle_2016_07 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(118, 260);

            AddTest(@"abba[mnop]qrst.
abcd[bddb]xyyx.
aaaa[qwer]tyui.
ioxxoj[asdfgh]zxcvbn", 2, 0);
            AddTest(@"aba[bab]xyz
xyx[xyx]xyx
aaa[kek]eke
zazbz[bzb]cdb", 0, 3);
        }

        private string[] Ips { get; }

        public Puzzle_2016_07(string input)
        {
            Ips = input
                .Lines();
        }

        protected override object Part1()
        {
            return Ips.Count(ip => ip.Matches("\\[[a-z]+\\]").None(HasAbba) && HasAbba(ip));
        }

        private static bool HasAbba(string ipPart)
        {
            if (ipPart.Length < 4) return false;

            for (var i = 0; i < ipPart.Length - 3; i++)
            {
                if (ipPart[i] != ipPart[i + 1] && ipPart[i] == ipPart[i + 3] && ipPart[i + 1] == ipPart[i + 2]) return true;
            }

            return false;
        }

        protected override object Part2()
        {
            var count = 0;

            foreach (var ip in Ips)
            {
                var hypernetSequences = ip.Matches("\\[[a-z]+\\]");

                foreach (var bab in ip.SplitByMatches("\\[[a-z]+\\]").SelectMany(GetBabsToMatch))
                {
                    if(hypernetSequences.Any(m => m.HasMatch(bab)))
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }

        private IEnumerable<string> GetBabsToMatch(string ipPart)
        {
            var babs = new HashSet<string>();

            if (ipPart.Length < 3) return babs;


            for (var i = 0; i < ipPart.Length - 2; i++)
            {
                if (ipPart[i] != ipPart[i + 1] && ipPart[i] == ipPart[i + 2])
                {
                    babs.Add($"{ipPart[i + 1]}{ipPart[i]}{ipPart[i + 1]}");
                }
            }

            return babs;
        }
    }
}