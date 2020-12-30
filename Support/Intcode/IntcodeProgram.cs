using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Support.Intcode
{
    public class IntcodeProgram
    {
        public Memory Memory { get; }

        private long _instructionPointer;
        private long _relativeBase;

        private int _nextInputIndex;
        public long NextInputIndex => _nextInputIndex;
        private readonly List<long> _inputs = new();
        public Func<int, long>? InputProvider { get; set; }

        private readonly List<long> _outputs = new();
        public IReadOnlyList<long> Outputs => _outputs;
        public long LastOutputIndex => _outputs.Count -1;
        public long LastOutput => _outputs.Last();
        public Action<long, int>? OutputHandler;

        public bool IsTerminated { get; private set; }

        public IntcodeProgram(string sourceCode, params long[] initialInputs)
        {
            Memory = new Memory();
            Memory.WriteRangeTo(0, sourceCode.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray());
            _inputs.AddRange(initialInputs);
        }

        public IntcodeProgram(IntcodeProgram other, params long[] initialInputs)
        {
            Memory = other.Memory.Snapshot();
            _instructionPointer = other._instructionPointer;
            _relativeBase = other._relativeBase;
            _nextInputIndex = other._nextInputIndex;
            _inputs.AddRange(other._inputs);
            _inputs.AddRange(initialInputs);
            InputProvider = other.InputProvider;
            _outputs.AddRange(other._outputs);
            OutputHandler = other.OutputHandler;
            IsTerminated = other.IsTerminated;
        }

        public void AddInput(long value)
        {
            _inputs.Add(value);
        }

        public void AddInputs(IEnumerable<long> values)
        {
            foreach (var value in values)
            {
                _inputs.Add(value);
            }
        }

        public (bool terminated, InterruptMode interruptType) Step(InterruptMode interruptMode = InterruptMode.Never)
        {
            if (IsTerminated) return (true, InterruptMode.Never);

            while (true)
            {
                var context = new ExecutionContext(Memory, _instructionPointer, _relativeBase);

                Action<ExecutionContext> stepOver = context.Instruction switch
                {
                    Instruction.Add => ctx => ctx.Arg3 = ctx.Arg1 + ctx.Arg2,
                    Instruction.Multiply => ctx => ctx.Arg3 = ctx.Arg1 * ctx.Arg2,
                    Instruction.ReadInput => ctx => ctx.Arg1 = ReadInput(),
                    Instruction.WriteOutput => ctx => WriteOutput(context.Arg1),
                    Instruction.JumpIfNotZero => ctx => { if (ctx.Arg1 != 0) ctx.JumpTo = ctx.Arg2; }
                    ,
                    Instruction.JumpIfZero => ctx => { if (ctx.Arg1 == 0) ctx.JumpTo = ctx.Arg2; }
                    ,
                    Instruction.IsLesThan => ctx => ctx.Arg3 = ctx.Arg1 < ctx.Arg2 ? 1 : 0,
                    Instruction.IsEqual => ctx => ctx.Arg3 = ctx.Arg1 == ctx.Arg2 ? 1 : 0,
                    Instruction.SetRelativeBase => ctx => ctx.RelativeBaseOffset = ctx.Arg1,
                    Instruction.Terminate => ctx => ctx.Terminated = true,
                    _ => throw new Exception($"Invalid {nameof(Instruction)}")
                };

                stepOver(context);

                _relativeBase += (int)(context.RelativeBaseOffset ?? 0);
                _instructionPointer = (int)(context.JumpTo ?? context.InstructionSize + _instructionPointer);

                if (IsTerminated = context.Terminated) return (IsTerminated, InterruptMode.Never);

                if ((context.InterruptType & interruptMode) == context.InterruptType) return (IsTerminated, context.InterruptType);
            }
        }

        private long ReadInput()
        {
            if (_nextInputIndex >= _inputs.Count)
            {
                if (InputProvider is null) throw new Exception("Could not read input");

                while (_nextInputIndex >= _inputs.Count)
                {
                    _inputs.Add(InputProvider(_inputs.Count));
                }
            }

            return _inputs[_nextInputIndex++];
        }

        private void WriteOutput(long value)
        {
            _outputs.Add(value);
            OutputHandler?.Invoke(value, _outputs.Count - 1);
        }

        public void Run()
        {
            Step();
        }
    }
}
