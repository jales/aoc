using System;

namespace AoC.Support.Intcode
{
    internal class ExecutionContext
    {
        private readonly Memory _memory;
        private readonly long _instructionPointer;
        private readonly long _relativeBase;

        public Instruction Instruction { get; }

        private readonly ArgumentMode _arg1Mode;
        public long Arg1
        {
            get => ReadArgument(1, _arg1Mode);
            set => WriteArgument(1, _arg1Mode, value);
        }

        private readonly ArgumentMode _arg2Mode;

        public long Arg2
        {
            get => ReadArgument(2, _arg2Mode);
            set => WriteArgument(2, _arg2Mode, value);
        }

        private readonly ArgumentMode _arg3Mode;
        public long Arg3
        {
            get => ReadArgument(3, _arg3Mode);
            set => WriteArgument(3, _arg3Mode, value);
        }

        public long? JumpTo { get; set; }
        public long? RelativeBaseOffset { get; set; }

        public bool Terminated { get; set; }

        public ExecutionContext(Memory memory, long instructionPointer, long relativeBase)
        {
            _memory = memory;
            _instructionPointer = instructionPointer;
            _relativeBase = relativeBase;
            JumpTo = null;

            var code = _memory.ReadFrom(instructionPointer);
            Instruction = (Instruction)(code % 100);
            _arg1Mode = (ArgumentMode)((code / 100) % 10);
            _arg2Mode = (ArgumentMode)((code / 1000) % 10);
            _arg3Mode = (ArgumentMode)((code / 10000) % 10);
        }

        public int InstructionSize => Instruction switch
        {
            Instruction.Add => 4,
            Instruction.Multiply => 4,
            Instruction.ReadInput => 2,
            Instruction.WriteOutput => 2,
            Instruction.JumpIfNotZero => 3,
            Instruction.JumpIfZero => 3,
            Instruction.IsLesThan => 4,
            Instruction.IsEqual => 4,
            Instruction.SetRelativeBase => 2,
            Instruction.Terminate => 1,
            _ => throw new ArgumentException($"Invalid {nameof(Instruction)}", nameof(Instruction))
        };

        public InterruptMode InterruptType => Instruction switch
        {
            Instruction.Add => InterruptMode.Arithmetic,
            Instruction.Multiply => InterruptMode.Arithmetic,
            Instruction.ReadInput => InterruptMode.Input,
            Instruction.WriteOutput => InterruptMode.Output,
            Instruction.JumpIfNotZero => InterruptMode.Jump,
            Instruction.JumpIfZero => InterruptMode.Jump,
            Instruction.IsLesThan => InterruptMode.Test,
            Instruction.IsEqual => InterruptMode.Test,
            Instruction.SetRelativeBase => InterruptMode.Control,
            Instruction.Terminate => InterruptMode.Control,
            _ => throw new ArgumentException($"Invalid {nameof(Instruction)}", nameof(Instruction))
        };

        private long ReadArgument(long argumentOffset, ArgumentMode mode)
        {
            var value = _memory.ReadFrom(_instructionPointer + argumentOffset);

            return mode switch
            {
                ArgumentMode.Immediate => value,
                ArgumentMode.Position => _memory.ReadFrom(value),
                ArgumentMode.Relative => _memory.ReadFrom(_relativeBase + value),
                _ => throw new ArgumentException($"Invalid {nameof(ArgumentMode)}", nameof(mode))
            };
        }

        private void WriteArgument(long argumentOffset, ArgumentMode mode, long value)
        {
            var address = _memory.ReadFrom(_instructionPointer + argumentOffset);

            switch (mode)
            {
                case ArgumentMode.Position:
                    _memory.WriteTo(address, value);
                    break;
                case ArgumentMode.Relative:
                    _memory.WriteTo(_relativeBase + address, value);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(ArgumentMode)}", nameof(mode));
            }
        }
    }
}
