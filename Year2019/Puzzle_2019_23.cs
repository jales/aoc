using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;
using static System.Linq.Enumerable;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_23 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(22134, 16084);
        }

        private IntcodeProgram OriginalProgram { get; }

        public Puzzle_2019_23(string input)
        {
            OriginalProgram = new IntcodeProgram(input);
        }

        protected override object Part1()
        {
            var computers = BuildComputers();

            long? firstYToNat = null;

            foreach (var (computer, _) in computers)
            {
                computer.OutputHandler = (_, ii) =>
                {
                    {
                        if ((ii + 1) % 3 != 0) return;

                        var (address, x, y) = (computer.Outputs[^3], computer.Outputs[^2], computer.Outputs[^1]);

                        if (address < computers.Length)
                        {
                            computers[address].inputs.Enqueue(x);
                            computers[address].inputs.Enqueue(y);
                        }

                        if (address == 255)
                        {
                            firstYToNat ??= y;
                        }
                    }
                };
            }

            while (true)
            {
                for (var i = 0; i < 50; i++)
                {
                    var computer = computers[i].computer;
                    if (computer.Step(InterruptMode.InputAndOutput).interruptType == InterruptMode.Output)
                    {
                        computer.Step(InterruptMode.Output);
                        computer.Step(InterruptMode.Output);
                    }
                }

                if (firstYToNat.HasValue) break;
            }

            return firstYToNat;
        }

        protected override object Part2()
        {
            var computers = BuildComputers();

            (long x, long y)? lastNatPacket = null;
            long? lastYSentByNat = null;

            foreach (var (computer, _) in computers)
            {
                computer.OutputHandler = (_, ii) =>
                {
                    if ((ii + 1) % 3 != 0) return;

                    var (address, x, y) = (computer.Outputs[^3], computer.Outputs[^2], computer.Outputs[^1]);

                    if (address < computers.Length)
                    {
                        computers[address].inputs.Enqueue(x);
                        computers[address].inputs.Enqueue(y);
                    }

                    if (address == 255)
                    {
                        lastNatPacket = (x, y);
                    }
                };
            }

            while (true)
            {
                for (var i = 0; i < 50; i++)
                {
                    var computer = computers[i].computer;
                    if (computer.Step(InterruptMode.InputAndOutput).interruptType == InterruptMode.Output)
                    {
                        computer.Step(InterruptMode.Output);
                        computer.Step(InterruptMode.Output);
                    }
                }

                if (lastNatPacket.HasValue && computers.All(x => x.inputs.Count == 0))
                {
                    if (lastYSentByNat == lastNatPacket.Value.y) break;

                    computers[0].inputs.Enqueue(lastNatPacket.Value.x);
                    computers[0].inputs.Enqueue(lastNatPacket.Value.y);

                    lastYSentByNat = lastNatPacket.Value.y;

                    lastNatPacket = null;
                }
            }

            return lastYSentByNat;
        }

        private (IntcodeProgram computer, Queue<long> inputs)[] BuildComputers()
        {
            return Range(0, 50).Select(BuildComputer).ToArray();
        }

        private (IntcodeProgram computer, Queue<long> inputs) BuildComputer(int index)
        {
            var inputs = new Queue<long>();
            inputs.Enqueue(index);

            var computer = new IntcodeProgram(OriginalProgram)
            {
                InputProvider = _ =>
                {
                    var result = inputs.TryDequeue(out var i) ? i : -1L;
                    return result;
                },
            };

            computer.Step(InterruptMode.Input);

            return (computer, inputs);
        }
    }
}