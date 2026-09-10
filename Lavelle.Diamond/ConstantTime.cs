using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Diamond
{
    public class ConstantTime
    {
        public static uint ExtractUpperBits(ulong value) => (uint)(value >> 32);
        public static uint ExtractOverflowBit(ulong value) => (uint)(value >> 32 & 1);
        public static uint IsPositive(int value) => (uint)(-value >> 31) & 1;
        public static uint IsNonNegative(int value) => ~(uint)(value >> 31) & 1;
        public static uint GreaterThan(int a, int b) => IsPositive((int)((long)a - b));
        public static uint IsNonZero(uint value) => (uint)((value | -value) >> 31 & 1);
        public static uint IsZero(uint value) => 1 - IsNonZero(value);
        public static uint Not(uint value) => 1 - value;
        public static uint IsOdd(uint value) => value & 1;
        public static uint IsEven(uint value) => 1 - IsOdd(value);

        public static uint Select(uint condition, uint a, uint b)
        {
            uint mask = (uint)-(int)condition;
            return a & mask | b & ~mask;
        }
        public static ulong Select(uint condition, ulong a, ulong b)
        {
            ulong mask = (ulong)-condition;
            return a & mask | b & ~mask;
        }

        public static uint TryGetLimb(uint[] limbs, int i, uint elseVal)
        {
            var inBounds = GreaterThan(limbs.Length, i) & IsNonNegative(i);
            var index = Select(inBounds, (uint)i, 0);
            return Select(inBounds, limbs[(int)index], elseVal);
        }

        public static void TrySetLimb(uint[] limbs, int i, uint value)
        {
            var atZero = limbs[0];
            var inBounds = GreaterThan(limbs.Length, i) & IsNonNegative(i);
            var index = Select(inBounds, (uint)i, 0);
            limbs[(int)index] = Select(inBounds, value, atZero);
        }

        public static void Copy(uint[] source, int sourceIndex, uint[] dest, int destIndex, int LimbCount)
        {
            for (int i = 0; i < LimbCount; i++) dest[destIndex + i] = source[sourceIndex + i];
        }

        public static int CountTrailingZeros(uint[] limbs)
        {
            var count = 0;
            var foundNonZero = 0U;

            for (int i = 0; i < limbs.Length; i++)
            {
                var limbTrailing = LimbCountTrailingZeros(limbs[i]);
                var limbIsZero = IsZero(limbs[i]);
                var contribution = Select(limbIsZero, 32U, limbTrailing);
                var toAdd = Select(foundNonZero, 0U, contribution);
                count += (int)toAdd;
                var limbIsNonZero = IsNonZero(limbs[i]);
                foundNonZero |= limbIsNonZero;
            }

            return count;
        }

        private static uint LimbCountTrailingZeros(uint value)
        {
            var count = 0U;
            var stopCounting = 0U;

            for (int bit = 0; bit < 32; bit++)
            {
                var bitIsZero = IsZero(value & 1);
                var shouldIncrement = bitIsZero & ~stopCounting;
                count += shouldIncrement;

                var bitIsOne = IsNonZero(value & 1);
                stopCounting |= bitIsOne;
                value >>= 1;
            }

            return count;
        }

        public static int CountLeadingZeros(uint value)
        {
            var count = 0;
            var stopCounting = 0U;

            for (int bit = 31; bit >= 0; bit--)
            {
                var bitIsZero = IsZero(value >> bit & 1);
                var shouldIncrement = bitIsZero & ~stopCounting;
                count += (int)shouldIncrement;

                var bitIsOne = IsNonZero(value >> bit & 1);
                stopCounting |= bitIsOne;
            }

            return count;
        }
    }
}
