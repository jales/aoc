namespace AoC.Support.Intcode
{
    internal enum Instruction
    {
        Add = 1,
        Multiply = 2,
        ReadInput = 3,
        WriteOutput = 4,
        JumpIfNotZero = 5,
        JumpIfZero = 6,
        IsLesThan = 7,
        IsEqual = 8,
        SetRelativeBase = 9,
        Terminate = 99,
    }
}
