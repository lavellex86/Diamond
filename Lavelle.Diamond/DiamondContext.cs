using ILGPU;
using Lavelle.Lazulite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Diamond
{
    public class DiamondContext(LazuliteContext? lctx = null)
    {
        public LazuliteContext lctx { get; } = lctx ?? new();

        internal static uint RightRotate(uint x, int n) => (x >> n) | (x << (32 - n));
        internal static uint LeftRotate(uint x, int n) => (x << n) | (x >> (32 - n));
        internal static uint Choose(uint x, uint y, uint z) => (x & y) ^ (~x & z);
        internal static uint Majority(uint x, uint y, uint z) => (x & y) ^ (x & z) ^ (y & z);
        internal static uint USigma0(uint x) => RightRotate(x, 2) ^ RightRotate(x, 13) ^ RightRotate(x, 22);
        internal static uint USigma1(uint x) => RightRotate(x, 6) ^ RightRotate(x, 11) ^ RightRotate(x, 25);
        internal static uint LSigma0(uint x) => RightRotate(x, 7) ^ RightRotate(x, 18) ^ (x >> 3);
        internal static uint LSigma1(uint x) => RightRotate(x, 17) ^ RightRotate(x, 19) ^ (x >> 10);

        internal static ulong RightRotate(ulong x, int n) => (x >> n) | (x << (64 - n));
        internal static ulong Choose(ulong x, ulong y, ulong z) => (x & y) ^ (~x & z);
        internal static ulong Majority(ulong x, ulong y, ulong z) => (x & y) ^ (x & z) ^ (y & z);
        internal static ulong USigma0(ulong x) => RightRotate(x, 28) ^ RightRotate(x, 34) ^ RightRotate(x, 39);
        internal static ulong USigma1(ulong x) => RightRotate(x, 14) ^ RightRotate(x, 18) ^ RightRotate(x, 41);
        internal static ulong LSigma0(ulong x) => RightRotate(x, 1) ^ RightRotate(x, 8) ^ (x >> 7);
        internal static ulong LSigma1(ulong x) => RightRotate(x, 19) ^ RightRotate(x, 61) ^ (x >> 6);

        internal static int[] PrimesTo80th() =>
        [
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53,
            59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131,
            137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
            227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311,
            313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409
        ];
    }
}
