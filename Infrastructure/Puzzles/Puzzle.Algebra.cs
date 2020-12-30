using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using MathNet.Numerics;

namespace AoC.Infrastructure.Puzzles
{
    public abstract partial class Puzzle
    {
        #region Canonical Modulus

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Mod(double dividend, double divisor) => Euclid.Modulus(dividend, divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Mod(float dividend, float divisor) => Euclid.Modulus(dividend, divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Mod(int dividend, int divisor) => Euclid.Modulus(dividend, divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Mod(long dividend, long divisor) => Euclid.Modulus(dividend, divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Mod(BigInteger dividend, BigInteger divisor) => Euclid.Modulus(dividend, divisor);

        #endregion

        #region Greatest Common Divisor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Gcd(long a, long b) => Euclid.GreatestCommonDivisor(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Gcd(IList<long> integers) => Euclid.GreatestCommonDivisor(integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Gcd(params long[] integers) => Euclid.GreatestCommonDivisor((IList<long>)integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Gcd(BigInteger a, BigInteger b) => BigInteger.GreatestCommonDivisor(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Gcd(IList<BigInteger> integers) => Euclid.GreatestCommonDivisor(integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Gcd(params BigInteger[] integers) => Euclid.GreatestCommonDivisor((IList<BigInteger>)integers);

        #endregion

        #region Extended Greatest Common Divisor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (long gcd, long x, long y) Egcd(long a, long b)
        {
            var gcd = Euclid.ExtendedGreatestCommonDivisor(a, b, out var x, out var y);

            return (gcd, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (BigInteger gcd, BigInteger x, BigInteger y) Egcd(BigInteger a, BigInteger b)
        {
            var gcd = Euclid.ExtendedGreatestCommonDivisor(a, b, out var x, out var y);

            return (gcd, x, y);
        }

        #endregion

        #region Least Common Multiple

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Lcm(long a, long b) => Euclid.LeastCommonMultiple(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Lcm(IList<long> integers) => Euclid.LeastCommonMultiple(integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Lcm(params long[] integers) => Euclid.LeastCommonMultiple((IList<long>) integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Lcm(BigInteger a, BigInteger b) => Euclid.LeastCommonMultiple(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Lcm(IList<BigInteger> integers) => Euclid.LeastCommonMultiple(integers);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Lcm(params BigInteger[] integers) => Euclid.LeastCommonMultiple((IList<BigInteger>) integers);

        #endregion

        #region Modular Inverse

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ModInv(long a, long b)
        {
            var (gcd, x, y) = Egcd(a, b);

            if (gcd != 1) throw new InvalidOperationException("Modular inverse does not exit");

            return Mod(x, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ModInv(BigInteger a, BigInteger b)
        {
            var (gcd, x, y) = Egcd(a, b);

            if (gcd != 1) throw new InvalidOperationException("Modular inverse does not exit");

            return  Mod(x, b);
        }

        #endregion

        #region Modular Exponentiation

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus) => BigInteger.ModPow(value, exponent, modulus);

        #endregion
    }
}