using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_21 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(78, 148);
        }

        private readonly List<(int cost, int damage, int armor)> _weapons = new()
        {
            ( 8, 4, 0),
            (10, 5, 0),
            (25, 6, 0),
            (40, 7, 0),
            (74, 8, 0)
        };

        private readonly List<(int cost, int damage, int armor)> _armor = new()
        {
            ( 13, 0, 1),
            ( 31, 0, 2),
            ( 53, 0, 3),
            ( 75, 0, 4),
            (102, 0, 5)
        };

        private readonly List<(int cost, int damage, int armor)> _rings = new()
        {
            ( 25, 1, 0),
            ( 50, 2, 0),
            (100, 3, 0),
            ( 20, 0, 1),
            ( 40, 0, 2),
            ( 80, 0, 3)
        };

        private int BossHitPoints { get; }
        private int BossDamage { get; }
        private int BossArmor { get; }

        public Puzzle_2015_21(string input)
        {
            var lines = input.Lines();

            BossHitPoints = lines[0].Int32Matches()[0];
            BossDamage = lines[1].Int32Matches()[0];
            BossArmor = lines[2].Int32Matches()[0];
        }

        protected override object Part1()
        {
            var cheapestWinningSetup = int.MaxValue;
            foreach (var weapon in _weapons)
            foreach (var armor in _armor.Prepend<(int cost, int damage, int armor)> ((0, 0, 0)))
            foreach (var rings in _rings.Combinations(1).Concat(_rings.Combinations(2)).Prepend(new List<(int cost, int damage, int armor)> {(0, 0, 0)}))
            {
                var totalCost =
                    weapon.cost +
                    armor.cost +
                    rings.Sum(r => r.cost);

                if (totalCost >= cheapestWinningSetup)
                {
                    continue;
                }

                var totalDamage =
                    weapon.damage +
                    armor.damage +
                    rings.Sum(r => r.damage);

                var totalArmor =
                    weapon.armor +
                    armor.armor +
                    rings.Sum(r => r.armor);

                var damagePerRound = Math.Max(1, totalDamage - BossArmor);
                var rounds = (BossHitPoints / damagePerRound) + (BossHitPoints % damagePerRound != 0 ? 1 : 0);

                var bossDamagePerRound = Math.Max(1, BossDamage - totalArmor);
                var bossRounds = (100 / bossDamagePerRound) + (100 % bossDamagePerRound != 0 ? 1 : 0);

                if (rounds <= bossRounds)
                {
                    cheapestWinningSetup = totalCost;
                }
            }

            return cheapestWinningSetup;
        }

        protected override object Part2()
        {
            var mostExpensiveLosingSetup = int.MinValue;
            foreach (var weapon in _weapons)
            foreach (var armor in _armor.Prepend<(int cost, int damage, int armor)> ((0, 0, 0)))
            foreach (var rings in _rings.Combinations(1).Concat(_rings.Combinations(2)).Prepend(new List<(int cost, int damage, int armor)> {(0, 0, 0)}))
            {
                var totalCost =
                    weapon.cost +
                    armor.cost +
                    rings.Sum(r => r.cost);

                if (totalCost <= mostExpensiveLosingSetup)
                {
                    continue;
                }

                var totalDamage =
                    weapon.damage +
                    armor.damage +
                    rings.Sum(r => r.damage);

                var totalArmor =
                    weapon.armor +
                    armor.armor +
                    rings.Sum(r => r.armor);

                var damagePerRound = Math.Max(1, totalDamage - BossArmor);
                var rounds = (BossHitPoints / damagePerRound) + (BossHitPoints % damagePerRound != 0 ? 1 : 0);

                var bossDamagePerRound = Math.Max(1, BossDamage - totalArmor);
                var bossRounds = (100 / bossDamagePerRound) + (100 % bossDamagePerRound != 0 ? 1 : 0);

                if (rounds > bossRounds)
                {
                    mostExpensiveLosingSetup = totalCost;
                }
            }

            return mostExpensiveLosingSetup;
        }
    }
}