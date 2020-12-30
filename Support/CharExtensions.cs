using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class CharExtensions
    {
        private static HashSet<char> Vowels { get; } = new() {'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U'};

        public static bool IsVowel(this in char source)
        {
            return Vowels.Contains(source);
        }
    }
}
