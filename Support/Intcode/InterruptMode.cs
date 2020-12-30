using System;

namespace AoC.Support.Intcode
{
    [Flags]
    public enum InterruptMode
    {
        Never = 0,
        Input = 1,
        Output = 2,
        Arithmetic = 4,
        Jump = 8,
        Test = 16,
        Control = 32,
        InputAndOutput = Input | Output,
        AllInstructions = InputAndOutput | Arithmetic | Jump | Test | Control
    }
}
