using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_22 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(953, 1289);
        }

        private PriorityQueue<GameState> StatesToProcess { get; }

        public Puzzle_2015_22(string input)
        {
            StatesToProcess = new PriorityQueue<GameState>(Comparer<GameState>.Create((x, y) => y.TotalManaSpent.CompareTo(x.TotalManaSpent)));

            var lines = input.Lines();

            StatesToProcess.Enqueue(new GameState
            {
                IsPlayerTurn = true,
                BossHealth = lines[0].Int32Matches()[0],
                BossDamage = lines[1].Int32Matches()[0],
                PlayerHealth = 50,
                PlayerMana = 500,
                Affects = new Dictionary<Affect, int>
                {
                    [Affect.Shield] = 0,
                    [Affect.Poison] = 0,
                    [Affect.Recharge] = 0
                }
            });
        }

        protected override object Part1()
        {
            var cheapestRun = int.MaxValue;

            while (StatesToProcess.TryDequeue(out var stateToProcess))
            {
                if (stateToProcess.TotalManaSpent >= cheapestRun) continue;

                foreach (var newState in Simulate(stateToProcess, hardMode: false))
                {
                    if (newState.PlayerIsDead) continue;

                    if (newState.BossIsDead) { cheapestRun = Math.Min(cheapestRun, newState.TotalManaSpent); }
                    else                     { StatesToProcess.Enqueue(newState);                            }
                }
            }

            return cheapestRun;
        }

        protected override object Part2()
        {
            var cheapestRun = int.MaxValue;

            while (StatesToProcess.TryDequeue(out var stateToProcess))
            {
                if (stateToProcess.TotalManaSpent >= cheapestRun) continue;

                foreach (var newState in Simulate(stateToProcess, hardMode: true))
                {
                    if (newState.PlayerIsDead) continue;

                    if (newState.BossIsDead) { cheapestRun = Math.Min(cheapestRun, newState.TotalManaSpent); }
                    else                     { StatesToProcess.Enqueue(newState);                            }
                }
            }

            return cheapestRun;
        }

        private static IEnumerable<GameState> Simulate(GameState stateToProcess, bool hardMode) => stateToProcess switch
        {
            { IsPlayerTurn: false } => SimulateBoss(stateToProcess),
            { IsPlayerTurn: true } => SimulatePlayer(stateToProcess, hardMode),
        };

        private static IEnumerable<GameState> SimulateBoss(GameState stateToProcess)
        {
            ApplyAffects(stateToProcess);

            return stateToProcess switch
            {
                { BossIsDead: true } => new [] {
                    stateToProcess.With(s =>
                    {
                        s.IsPlayerTurn = true;
                    })
                },

                { BossIsDead: false} => new [] {
                    stateToProcess.With(s =>
                    {
                        s.IsPlayerTurn = true;
                        s.PlayerHealth -= Math.Max(stateToProcess.BossDamage - stateToProcess.PlayerArmor, 1);
                    })
                }
            };
        }

        private static IEnumerable<GameState> SimulatePlayer(GameState stateToProcess, bool hardMode)
        {
            if (hardMode)
            {
                stateToProcess.PlayerHealth -= 1;
                if (stateToProcess.PlayerIsDead)
                {
                    return new[] { stateToProcess };
                }
            }

            var states = new List<GameState>();

            ApplyAffects(stateToProcess);

            if (stateToProcess.BossIsDead)
            {
               states.Add(stateToProcess.With(s =>
                {
                    s.IsPlayerTurn = false;
                }));
            }
            else
            {
                if (stateToProcess.PlayerMana >= 53) states.Add(stateToProcess.With(CastMagicMissile));

                if (stateToProcess.PlayerMana >= 73)states.Add(stateToProcess.With(CastDrain));

                if (stateToProcess.PlayerMana >= 113 && stateToProcess.Affects[Affect.Shield] == 0) states.Add(stateToProcess.With(CastShield));

                if (stateToProcess.PlayerMana >= 173 && stateToProcess.Affects[Affect.Poison] == 0) states.Add(stateToProcess.With(CastPoison));

                if (stateToProcess.PlayerMana >= 229 && stateToProcess.Affects[Affect.Recharge] == 0) states.Add(stateToProcess.With(CastRecharge));
            }

            return states;
        }

        private static void CastMagicMissile(GameState s)
        {
            s.IsPlayerTurn = false;
            s.BossHealth -= 4;
            s.PlayerMana -= 53;
            s.TotalManaSpent += 53;
        }

        private static void CastDrain(GameState s)
        {
            s.IsPlayerTurn = false;
            s.PlayerHealth += 2;
            s.BossHealth -= 2;
            s.PlayerMana -= 73;
            s.TotalManaSpent += 73;
        }

        private static void CastShield(GameState s)
        {
            s.IsPlayerTurn = false;
            s.Affects[Affect.Shield] = 6;
            s.PlayerMana -= 113;
            s.TotalManaSpent += 113;
        }

        private static void CastPoison(GameState s)
        {
            s.IsPlayerTurn = false;
            s.Affects[Affect.Poison] = 6;
            s.PlayerMana -= 173;
            s.TotalManaSpent += 173;
        }

        private static void CastRecharge(GameState s)
        {
            s.IsPlayerTurn = false;
            s.Affects[Affect.Recharge] = 5;
            s.PlayerMana -= 229;
            s.TotalManaSpent += 229;
        }

        private static void ApplyAffects(GameState stateToProcess)
        {
            ApplyAffect(stateToProcess, Affect.Shield, s => s.PlayerArmor = 7);
            ApplyAffect(stateToProcess, Affect.Poison, s => s.BossHealth -= 3);
            ApplyAffect(stateToProcess, Affect.Recharge, s => s.PlayerMana += 101);
        }

        private static void ApplyAffect(GameState stateToProcess, Affect affect, Action<GameState> affectAction)
        {
            if (stateToProcess.Affects[affect] <= 0) return;

            affectAction(stateToProcess);
            stateToProcess.Affects[affect] -= 1;
        }

        private enum Affect
        {
            Shield,
            Poison,
            Recharge
        }

        private class GameState
        {
            public bool IsPlayerTurn;

            public int BossHealth;
            public int BossDamage;

            public int PlayerHealth;
            public int PlayerMana;
            public int PlayerArmor;

            public int TotalManaSpent;

            public Dictionary<Affect, int> Affects = new Dictionary<Affect, int>();

            public bool BossIsDead => BossHealth <= 0;
            public bool PlayerIsDead => PlayerHealth <= 0;

            public GameState With(Action<GameState>? wither)
            {
                var newState = new GameState
                {
                    IsPlayerTurn = IsPlayerTurn,
                    BossHealth = BossHealth,
                    BossDamage = BossDamage,
                    PlayerHealth = PlayerHealth,
                    PlayerMana = PlayerMana,
                    PlayerArmor = 0,
                    TotalManaSpent = TotalManaSpent,
                    Affects = Affects.ToDictionary(x => x.Key, x => x.Value)
                };

                wither?.Invoke(newState);

                return newState;
            }
        }
    }
}