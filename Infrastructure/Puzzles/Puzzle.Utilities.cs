using System.Runtime.CompilerServices;

namespace AoC.Infrastructure.Puzzles
{
    public abstract partial class Puzzle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        protected static T[][] JaggedArray<T>(int dim1, int dim2)
        {
            var array = new T[dim1][];

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new T[dim2];
            }

            return array;
        }
    }
}